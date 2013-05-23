using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Linq;
using FubuCore.Conversion;

namespace FubuSaml2
{
    public class ReadsSamlXml
    {
        public static readonly string AssertionXsd = "urn:oasis:names:tc:SAML:2.0:assertion";
        public static readonly string ProtocolXsd = "urn:oasis:names:tc:SAML:2.0:protocol";

    }

    public class SamlResponseXmlReader : ReadsSamlXml
    {

        private readonly XmlDocument _document;

        public SamlResponseXmlReader(string xml)
        {
            _document = new XmlDocument();
            _document.LoadXml(xml);
        }

        private XmlElement find(string elementName, string xsd)
        {
            var elements = _document.GetElementsByTagName(elementName, xsd);
            return (XmlElement) (elements.Count > 0 ? elements[0] : null);

        }

        private string findText(string elementName, string xsd)
        {
            var element = find(elementName, xsd);
            return element == null ? null : element.InnerText;
        }

        // This assumes signing cert is embedded in the signature
        private void readSignaturesAndCertificates(SamlResponse response)
        {
            var element = find("Signature", "http://www.w3.org/2000/09/xmldsig#");
            if (element == null)
            {
                response.Signed = FubuSaml2.SignatureStatus.NotSigned;
                return;
            }
            
            var signedXml = new SignedXml(_document);
            signedXml.LoadXml(element);

            response.Signed = signedXml.CheckSignature()
                       ? FubuSaml2.SignatureStatus.Signed
                       : FubuSaml2.SignatureStatus.InvalidSignature;

            response.Certificate = signedXml.KeyInfo.OfType<KeyInfoX509Data>()
                                            .SelectMany(x => x.Certificates.OfType<X509Certificate2>());

            /*
            
            var signatureElement = (XmlElement)xmlDoc.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0];
            var signedXml = new SignedXml(xmlDoc);
            signedXml.LoadXml(signatureElement);
            var isValid = signedXml.CheckSignature();

            if (!isValid)
            {
                throw new Exception("Saml payload does not contain a valid signature");
            }
             */
        }

        public Uri readIssuer()
        {
            return findText("Issuer", AssertionXsd).ToUri();
        }

        public SamlResponse Read()
        {
            var response = new SamlResponse
            {
                Issuer = readIssuer(),
                Status = readStatusCode(),
                Conditions = new ConditionGroup(find("Conditions", AssertionXsd)),
                Subject = new Subject(find("Subject", AssertionXsd)),
            };

            readSignaturesAndCertificates(response);
            readAttributes(response);

            return response;
        }

        // TODO -- test payload w/o attributes
        private void readAttributes(SamlResponse response)
        {
            var attributes = find("AttributeStatement", AssertionXsd);
            if (attributes == null) return;

            foreach (XmlElement attElement in attributes.GetElementsByTagName("Attribute", AssertionXsd))
            {
                var key = attElement.GetAttribute("Name");
                foreach (XmlElement valueElement in attElement.GetElementsByTagName("AttributeValue", AssertionXsd))
                {
                    response.AddAttribute(key, valueElement.InnerText);
                }
            }
        }


        private SamlResponseStatus readStatusCode()
        {
            return find("StatusCode", ProtocolXsd).GetAttribute("Value").Split(':').Last().ToEnumValue<SamlResponseStatus>();
        }
    }

    public static class SamlBasicExtensions
    {
        private static readonly IObjectConverter converter = new ObjectConverter();

        public static IEnumerable<XmlElement> Children(this XmlElement element, string name, string xsd)
        {
            foreach (XmlNode node in element.GetElementsByTagName(name, SamlResponseXmlReader.AssertionXsd))
            {
                yield return (XmlElement) node;
            }
        }

        public static XmlElement FindChild(this XmlElement element, string name, string xsd)
        {
            var children = element.GetElementsByTagName(name, xsd);
            return (XmlElement) (children.Count > 0 ? children[0] : null);
        }

        public static Uri ToUri(this string uri)
        {
            return new Uri(uri);
        }

        public static T ToEnumValue<T>(this string text)
        {
            return (T) Enum.Parse(typeof (T), text);
        }

        public static T ReadAttribute<T>(this XmlElement element, string attribute) 
        {
            return element.HasAttribute(attribute) ? converter.FromString<T>(element.GetAttribute(attribute)) : default(T);
        }
    }
}