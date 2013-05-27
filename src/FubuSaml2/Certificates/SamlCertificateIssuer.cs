using System;

namespace FubuSaml2.Certificates
{
    public class SamlCertificateIssuer
    {
        public string SerialNumber { get; set; }
        public string CertificateIssuer { get; set; }
        public Uri SamlIssuer { get; set; }
        public string Thumbprint { get; set; }
    }
}