using System;
using System.Xml;

namespace FubuSaml2
{
    public class SamlResponseXmlReader
    {
        public static readonly string AssertionXsd = "urn:oasis:names:tc:SAML:2.0:assertion";

        private readonly XmlDocument _document;

        public SamlResponseXmlReader(string xml)
        {
            _document = new XmlDocument();
            _document.LoadXml(xml);
        }

        private XmlElement findAssertionElement(string elementName)
        {
            var elements = _document.GetElementsByTagName(elementName, AssertionXsd);
            return (XmlElement) (elements.Count > 0 ? elements[0] : null);

        }

        public Uri Issuer
        {
            get {
                var element = findAssertionElement("Issuer");
                return element == null ? null : new Uri(element.InnerText);
            }
        }
    }
}