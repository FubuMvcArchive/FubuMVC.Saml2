using System;
using System.Security.Cryptography.X509Certificates;

namespace FubuSaml2.Certificates
{
    public interface ICertificateIssuerMatcher
    {
        bool MatchesIssuer(Uri samlIssuer, X509Certificate2 signingCert);
    }
}