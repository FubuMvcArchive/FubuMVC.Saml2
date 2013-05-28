using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using FubuCore;
using FubuSaml2.Certificates;
using FubuSaml2.Xml;

namespace FubuSaml2.Encryption
{
    public class SamlResponseWriter : ReadsSamlXml
    {
        private readonly ICertificateService _certificates;
        private readonly ISamlResponseXmlSigner _xmlSigner;

        public SamlResponseWriter(ICertificateService certificates, ISamlResponseXmlSigner xmlSigner)
        {
            _certificates = certificates;
            _xmlSigner = xmlSigner;
        }

        public string Write(SamlResponse response)
        {
            var xml = new SamlResponseXmlWriter(response).Write();
            var certificate = _certificates.LoadCertificate(response.Issuer);

            _xmlSigner.ApplySignature(response, certificate, xml);

            var rawXml = xml.OuterXml;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(rawXml));
        }
    
    }

    // TODO -- need some tests around this
    public class SamlResponseReader : ReadsSamlXml
    {
        private readonly ICertificateService _certificates;

        public SamlResponseReader(ICertificateService certificates)
        {
            _certificates = certificates;
        }

        public SamlResponse Read(string responseText)
        {
            var bytes = Convert.FromBase64String(responseText);
            var xml = Encoding.UTF8.GetString(bytes);

            return new SamlResponseXmlReader(xml).Read();
        }

        public static void Decrypt(XmlDocument document, X509Certificate2 encryptionCert)
        {
            var assertion = document.FindChild("EncryptedAssertion");
            var data = document.EncryptedChild("EncryptedData");
            var keyElement = assertion.EncryptedChild("EncryptedKey");
            
            var encryptedData = new EncryptedData();
            encryptedData.LoadXml(data);
            
            var encryptedKey = new EncryptedKey();
            encryptedKey.LoadXml(keyElement);
            var encryptedXml = new EncryptedXml(document);

            // Get encryption secret key used by decrypting with the encryption certificate's private key
            var secretKey = GetSecretKey(encryptedKey, encryptionCert.PrivateKey);

            // Seed the decryption algorithm with secret key and then decrypt
            var algorithm = GetSymmetricBlockEncryptionAlgorithm(encryptedData.EncryptionMethod.KeyAlgorithm);
            algorithm.Key = secretKey;
            var decryptedBytes = encryptedXml.DecryptData(encryptedData, algorithm);

            // Put decrypted xml elements back into the document in place of the encrypted data
            encryptedXml.ReplaceData(assertion, decryptedBytes);
        }

        public static SymmetricAlgorithm GetSymmetricBlockEncryptionAlgorithm(string algorithmUri)
        {
            switch (algorithmUri)
            {
                case EncryptedXml.XmlEncTripleDESUrl:
                    return new TripleDESCryptoServiceProvider();
                case EncryptedXml.XmlEncDESUrl:
                    return new DESCryptoServiceProvider();
                case EncryptedXml.XmlEncAES128Url:
                    return new RijndaelManaged { KeySize = 128 };
                case EncryptedXml.XmlEncAES192Url:
                    return new RijndaelManaged { KeySize = 192 };
                case EncryptedXml.XmlEncAES256Url:
                    return new RijndaelManaged();
                default:
                    throw new Exception("Unrecognized symmetric encryption algorithm URI '{0}'".ToFormat(algorithmUri));
            }
        }

        public static byte[] GetSecretKey(EncryptedKey encryptedKey, AsymmetricAlgorithm privateKey)
        {
            var keyAlgorithm = encryptedKey.EncryptionMethod.KeyAlgorithm;
            var asymmetricAlgorithm = GetAsymmetricKeyTransportAlgorithm(keyAlgorithm);
            asymmetricAlgorithm.FromXmlString(privateKey.ToXmlString(true));
            var useOaep = keyAlgorithm == EncryptedXml.XmlEncRSAOAEPUrl;
            return asymmetricAlgorithm.Decrypt(encryptedKey.CipherData.CipherValue, useOaep);
        }

        public static RSACryptoServiceProvider GetAsymmetricKeyTransportAlgorithm(string algorithmUri)
        {
            switch (algorithmUri)
            {
                case EncryptedXml.XmlEncRSA15Url:
                case EncryptedXml.XmlEncRSAOAEPUrl:
                    return new RSACryptoServiceProvider();
                default:
                    throw new Exception("Unrecognized asymmetric encryption algorithm URI '{0}'".ToFormat(algorithmUri));
            }
        }
    }

    public interface ISamlResponseXmlSigner
    {
        void ApplySignature(SamlResponse response ,X509Certificate2 certificate, XmlDocument document);
    }

    public class SamlResponseXmlSigner : ReadsSamlXml, ISamlResponseXmlSigner
    {
        public void ApplySignature(SamlResponse response, X509Certificate2 certificate, XmlDocument document)
        {
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificate));

            var signedXml = new SignedXml(document)
            {
                SigningKey = certificate.PrivateKey,
                KeyInfo = keyInfo
            };

            var reference = new Reference("#" + response.Id);
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();

            var xml = signedXml.GetXml();

            document.FindChild(AssertionElem).AppendChild(xml);
        }
    }

    public class AssertionXmlEncryptor
    {
        public void Encrypt(XmlDocument document, X509Certificate2 certificate)
        {
            
        }

        public void Decrypt(XmlDocument document, X509Certificate2 certificate)
        {
            
        }
    }
}