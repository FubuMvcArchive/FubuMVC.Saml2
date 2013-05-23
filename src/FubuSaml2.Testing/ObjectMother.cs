using System.Security.Cryptography.X509Certificates;
using FubuSaml2.Certificates;

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
            var cert = X509Certificate2.CreateFromCertFile("cert2.cer");
            return new X509Certificate2(cert);
        }
    }
}