using System;
using System.Xml;
using System.Linq;
using FubuCore.Conversion;

namespace FubuSaml2
{
    public class SamlResponseXmlReader
    {
        public static readonly string AssertionXsd = "urn:oasis:names:tc:SAML:2.0:assertion";
        public static readonly string ProtocolXsd = "urn:oasis:names:tc:SAML:2.0:protocol";

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

        public Uri readIssuer()
        {
            return findText("Issuer", AssertionXsd).ToUri();
        }

        public SamlResponse Read()
        {
            var response = new SamlResponse
            {
                Issuer = readIssuer(),
                Status = readStatusCode().ToEnumValue<SamlResponseStatus>(),
                Conditions = readConditions()
            };

            return response;
        }

        private ConditionGroup readConditions()
        {
            var element = find("Conditions", AssertionXsd);
            var group = new ConditionGroup
            {
                NotBefore = element.ReadAttribute<DateTimeOffset>("NotBefore"),
                NotOnOrAfter = element.ReadAttribute<DateTimeOffset>("NotOnOrAfter"),
            };

            return group;
        }

        private string readStatusCode()
        {
            return find("StatusCode", ProtocolXsd).GetAttribute("Value").Split(':').Last();
        }
    }

    public static class SamlBasicExtensions
    {
        private static readonly IObjectConverter converter = new ObjectConverter();

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