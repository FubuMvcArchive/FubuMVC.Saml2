using System;

namespace FubuSaml2.Certificates
{
    public interface ISamlCertificateIssuerRepository
    {
        SamlCertificateIssuer Find(Uri samlIssuer);
    }
}