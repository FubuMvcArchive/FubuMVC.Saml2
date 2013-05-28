using System;

namespace FubuSaml2.Certificates
{
    public interface ISamlCertificateRepository
    {
        SamlCertificate Find(Uri samlIssuer);
    }
}