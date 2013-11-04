using System;
using System.Collections.Generic;

namespace FubuSaml2.Certificates
{
    public interface ISamlCertificateRepository
    {
        SamlCertificate Find(string samlIssuer);
        IEnumerable<SamlCertificate> AllKnownCertificates();
    }
}