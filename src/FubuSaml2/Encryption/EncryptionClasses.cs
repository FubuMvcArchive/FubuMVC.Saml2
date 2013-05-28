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

        public SamlResponseWriter(ICertificateService certificates)
        {
            _certificates = certificates;
        }

        public string Write(SamlResponse response)
        {
            var xml = new SamlResponseXmlWriter(response).Write();

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
}