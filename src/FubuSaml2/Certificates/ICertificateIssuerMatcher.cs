using System;

namespace FubuSaml2.Certificates
{
    public interface ICertificateIssuerMatcher
    {
        bool MatchesIssuer(Uri samlIssuer, ICertificate signingCert);
    }
}