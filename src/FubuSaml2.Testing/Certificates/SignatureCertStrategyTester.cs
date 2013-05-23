using System;
using System.Security.Cryptography.X509Certificates;
using FubuSaml2.Certificates;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuSaml2.Testing.Certificates
{
    [TestFixture]
    public class SignatureCertStrategyTester
    {
        private SamlCertificateIssuer[] issuers;
        private CertificateIssuerMatcher theStrategy;

        [SetUp]
        public void SetUp()
        {
            issuers = new SamlCertificateIssuer[]
            {
                new SamlCertificateIssuer{SamlIssuer = new Uri("foo:bar1")}, 
                new SamlCertificateIssuer{SamlIssuer = new Uri("foo:bar2")}, 
                new SamlCertificateIssuer{SamlIssuer = new Uri("foo:bar3"), SerialNumber = "123", CertificateIssuer = "the issuer"} 
            };

            var repository = new BasicSamlCertificateIssuerRepository(issuers);

            theStrategy = new CertificateIssuerMatcher(repository);
        }

        [Test]
        public void validate_postive_on_serial_number_and_signing_cert_issuer_match()
        {
            var cert = ObjectMother.Certificate1();
            issuers[2].CertificateIssuer = cert.Issuer;
            issuers[2].SerialNumber = cert.SerialNumber;

            theStrategy.MatchesIssuer(issuers[2].SamlIssuer, cert).ShouldBeTrue();
        }

        [Test]
        public void validate_negative_on_unknown_issuer()
        {
            theStrategy.MatchesIssuer(new Uri("unknown:issuer"), null).ShouldBeFalse();
        }

        [Test]
        public void validate_negative_on_signing_cert_issuer_mismatch()
        {
            var cert = ObjectMother.Certificate1();
            issuers[2].CertificateIssuer = cert.Issuer + "-different";
            issuers[2].SerialNumber = cert.SerialNumber;

            theStrategy.MatchesIssuer(issuers[2].SamlIssuer, cert).ShouldBeFalse();
        }

        [Test]
        public void validate_negative_on_serial_number_mismatch()
        {
            var cert = ObjectMother.Certificate1();
            issuers[2].CertificateIssuer = cert.Issuer;
            issuers[2].SerialNumber = cert.SerialNumber + "-more";

            theStrategy.MatchesIssuer(issuers[2].SamlIssuer, cert).ShouldBeFalse();
        }

    }
}