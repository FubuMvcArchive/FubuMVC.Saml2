using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Linq;
using FubuSaml2.Certificates;

namespace FubuSaml2.Xml
{
    public class ReadsSamlXml
    {
        public const string AssertionXsd = "urn:oasis:names:tc:SAML:2.0:assertion";
        public const string ProtocolXsd = "urn:oasis:names:tc:SAML:2.0:protocol";
        public const string EncryptedXsd = "http://www.w3.org/2001/04/xmlenc#";
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

            response.Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>()
                                            .SelectMany(x => x.Certificates.OfType<X509Certificate2>().Select(cert => new X509CertificateWrapper(cert)));

        }

        public Uri ReadIssuer()
        {
            return findText("Issuer", AssertionXsd).ToUri();
        }

        public SamlResponse Read()
        {
            var response = new SamlResponse
            {
                Id = _document.DocumentElement.GetAttribute("ID"),
                Destination = _document.DocumentElement.GetAttribute("Destination").ToUri(),
                IssueInstant = _document.DocumentElement.ReadAttribute<DateTimeOffset>("IssueInstant"),
                Issuer = ReadIssuer(),
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
}