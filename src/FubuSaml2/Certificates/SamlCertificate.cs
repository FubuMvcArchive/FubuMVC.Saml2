using System;

namespace FubuSaml2.Certificates
{
    public class SamlCertificate
    {
        public string SerialNumber { get; set; }
        public string CertificateIssuer { get; set; }
        public Uri Issuer { get; set; }
        public string Thumbprint { get; set; }
        public Uri Reference { get; set; }


        public bool Matches(ICertificate certificate)
        {
            return certificate.SerialNumber == SerialNumber &&
                   certificate.Issuer == CertificateIssuer;
        }
    }
}