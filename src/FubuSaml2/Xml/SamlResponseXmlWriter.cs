using System;
using System.Xml;

namespace FubuSaml2.Xml
{
    public class SamlResponseXmlWriter : ReadsSamlXml
    {
        public XmlDocument Write(SamlResponse response)
        {
            var nameTable = new NameTable();
            var namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("saml", AssertionXsd);
            namespaceManager.AddNamespace("samlp", ProtocolXsd);

            var document = new XmlDocument(nameTable);

            throw new NotImplementedException();
        }
    }
}