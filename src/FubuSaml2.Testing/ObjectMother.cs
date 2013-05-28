using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using FubuCore;
using FubuSaml2.Certificates;
using FubuSaml2.Xml;

namespace FubuSaml2.Testing
{
    public static class ObjectMother
    {
        public static ICertificate Certificate1()
        {
            var cert = X509Certificate2.CreateFromCertFile("cert1.cer");
            return new X509CertificateWrapper(new X509Certificate2(cert));
        }

        public static X509Certificate2 Certificate2()
        {
            var cert = new X509Certificate2("cert2.pfx", new SecureString(), X509KeyStorageFlags.Exportable);
            return new X509Certificate2(cert);
        }

        public static SamlResponse Response()
        {
            var xml = new FileSystem().ReadStringFromFile("sample.xml");
            return new SamlResponseXmlReader(xml).Read();
        }

        public static SamlCertificate SamlCertificateMatching(Uri issuer, ICertificate certificate)
        {
            if (certificate == null) throw new ArgumentNullException("certificate");
            return new SamlCertificate
            {
                CertificateIssuer = certificate.Issuer,
                SerialNumber = certificate.SerialNumber,
                Issuer = issuer
            };
        }
    }
}