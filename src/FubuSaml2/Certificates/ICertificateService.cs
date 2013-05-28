using System;
using System.Security.Cryptography.X509Certificates;

namespace FubuSaml2.Certificates
{
    public interface ICertificateService
    {
        CertificateResult Validate(SamlResponse response);
        X509Certificate2 LoadCertificate(Uri issuer);
    }
}