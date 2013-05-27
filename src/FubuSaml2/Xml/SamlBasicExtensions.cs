using System;
using System.Collections.Generic;
using System.Xml;
using FubuCore.Conversion;

namespace FubuSaml2.Xml
{
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

        public static XmlElement FindChild(this XmlElement element, string name, string xsd = SamlResponseXmlReader.AssertionXsd)
        {
            var children = element.GetElementsByTagName(name, xsd);
            return (XmlElement) (children.Count > 0 ? children[0] : null);
        }

        public static XmlElement FindChild(this XmlDocument document, string name, string xsd = SamlResponseXmlReader.AssertionXsd)
        {
            var element = document.DocumentElement;
            var children = element.GetElementsByTagName(name, xsd);
            return (XmlElement)(children.Count > 0 ? children[0] : null);
        }

        public static XmlElement EncryptedChild(this XmlDocument document, string name)
        {
            return document.FindChild(name, SamlResponseXmlReader.EncryptedXsd);
        }

        public static XmlElement EncryptedChild(this XmlElement element, string name)
        {
            return element.FindChild(name, SamlResponseXmlReader.EncryptedXsd);
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