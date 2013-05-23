using System;
using System.Security.Cryptography.X509Certificates;

namespace FubuSaml2.Certificates
{


    public class CertificateIssuerMatcher : ICertificateIssuerMatcher
    {
        private readonly ISamlCertificateIssuerRepository _repository;

        public CertificateIssuerMatcher(ISamlCertificateIssuerRepository repository)
        {
            _repository = repository;
        }

        public bool MatchesIssuer(Uri samlIssuer, ICertificate certificate)
        {
            var certIssuer = _repository.Find(samlIssuer);

            if (certIssuer == null) return false;

            return certificate.SerialNumber == certIssuer.SerialNumber &&
                   certificate.Issuer == certIssuer.CertificateIssuer;
        }
    }
}