using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuSaml2.Certificates
{
    public class BasicSamlCertificateRepository : ISamlCertificateRepository
    {
        private readonly IEnumerable<SamlCertificate> _issuers;

        public BasicSamlCertificateRepository(IEnumerable<SamlCertificate> issuers)
        {
            _issuers = issuers;
        }

        public SamlCertificate Find(Uri samlIssuer)
        {
            return _issuers.FirstOrDefault(x => x.Issuer == samlIssuer);
        }
    }
}