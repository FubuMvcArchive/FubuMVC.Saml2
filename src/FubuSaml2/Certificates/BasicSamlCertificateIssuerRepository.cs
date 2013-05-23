using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuSaml2.Certificates
{
    public class BasicSamlCertificateIssuerRepository : ISamlCertificateIssuerRepository
    {
        private readonly IEnumerable<SamlCertificateIssuer> _issuers;

        public BasicSamlCertificateIssuerRepository(IEnumerable<SamlCertificateIssuer> issuers)
        {
            _issuers = issuers;
        }

        public SamlCertificateIssuer Find(Uri samlIssuer)
        {
            return _issuers.FirstOrDefault(x => x.SamlIssuer == samlIssuer);
        }
    }
}