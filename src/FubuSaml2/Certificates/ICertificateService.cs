using System;
using System.Security.Cryptography.X509Certificates;
using FubuSaml2.Validation;

namespace FubuSaml2.Certificates
{
    public interface ICertificateService
    {
        SamlValidationKeys Validate(SamlResponse response);
        X509Certificate2 LoadCertificate(Uri issuer);
    }
}