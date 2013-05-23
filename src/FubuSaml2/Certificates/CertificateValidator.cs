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
            throw new NotImplementedException();
        }

        public bool MatchesIssuer(SamlResponse response)
        {
            var issuer = response.Issuer;
            return response.Certificates.Any(cert => {
                return _matchers.Any(x => x.MatchesIssuer(issuer, cert));
            });
        }

        /*
            public void ValidateSignature(XmlDocument xmlDoc)
            {
                // This assumes signing cert is embedded in the signature
                var signatureElement = (XmlElement)xmlDoc.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0];
                var signedXml = new SignedXml(xmlDoc);
                signedXml.LoadXml(signatureElement);
                var isValid = signedXml.CheckSignature();

                if (!isValid)
                {
                    throw new Exception("Saml payload does not contain a valid signature");
                }

                var samlResponseIssuerElement = (XmlElement)xmlDoc.GetElementsByTagName("Issuer", "urn:oasis:names:tc:SAML:2.0:assertion")[0];
                var samlIssuer = samlResponseIssuerElement.InnerText.Trim();
                var certStrategy = _signatureCertStrategies.FirstOrDefault(x => x.CanHandle(samlIssuer));

                if (certStrategy == null)
                {
                    throw new Exception("Unable to find an ICertificateIssuerMatcher for SAML Response Issuer '{0}'".ToFormat(samlIssuer));
                }

                var signingCert = signedXml.KeyInfo.OfType<KeyInfoX509Data>().Single().Certificates.OfType<X509Certificate2>().Single();

                if (!signingCert.Verify())
                {
                    throw new Exception("Saml signature was valid, but certificate used cannot not be verified as valid");
                }

                if (!certStrategy.MatchesIssuer(samlIssuer, signingCert))
                {
                    throw new Exception("Saml signature was valid, but not created with the expected certificate");
                }
            }
         */
    }
}