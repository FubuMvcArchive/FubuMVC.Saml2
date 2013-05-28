using System;
using System.Text;
using System.Xml;
using FubuSaml2.Certificates;
using FubuSaml2.Xml;

namespace FubuSaml2.Encryption
{
    public class SamlResponseReader : ReadsSamlXml
    {
        private readonly ICertificateService _certificates;
        private readonly IAssertionXmlDecryptor _decryptor;

        public SamlResponseReader(ICertificateService certificates, IAssertionXmlDecryptor decryptor)
        {
            _certificates = certificates;
            _decryptor = decryptor;
        }

        public SamlResponse Read(string responseText)
        {
            var bytes = Convert.FromBase64String(responseText);
            var xml = Encoding.UTF8.GetString(bytes);
            var document = new XmlDocument();
            document.LoadXml(xml);

            var reader = new SamlResponseXmlReader(document);
            var certificate = _certificates.LoadCertificate(reader.ReadIssuer());

            _decryptor.Decrypt(document, certificate);

            return reader.Read();
        }
    }
}