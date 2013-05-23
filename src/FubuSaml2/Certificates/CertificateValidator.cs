using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuSaml2.Certificates
{
    public class CertificateValidator : ICertificateValidator
    {
        private readonly IEnumerable<ICertificateIssuerMatcher> _matchers;

        public CertificateValidator(IEnumerable<ICertificateIssuerMatcher> matchers)
        {
            _matchers = matchers;
        }

        public CertificateResult Validate(SamlResponse response)
        {
            if (!MatchesIssuer(response)) return CertificateResult.CannotMatchIssuer;

            return response.Certificates.Any(x => x.IsVerified)
                       ? CertificateResult.Validated
                       : CertificateResult.NoValidCertificates;
        }

        // virtual for testing
        public virtual bool MatchesIssuer(SamlResponse response)
        {
            var issuer = response.Issuer;
            return response.Certificates.Any(cert => {
                return _matchers.Any(x => x.MatchesIssuer(issuer, cert));
            });
        }

    }
}