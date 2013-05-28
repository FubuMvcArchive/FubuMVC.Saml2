using System;
using FubuSaml2.Certificates;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuSaml2.Testing.Certificates
{
    [TestFixture]
    public class SamlCertificateTester
    {
        public readonly InMemoryCertificate Certificate = new InMemoryCertificate();

        [Test]
        public void matches_on_both_serial_number_and_certificate_issuer()
        {
            new SamlCertificate
            {
                SerialNumber = Certificate.SerialNumber,
                CertificateIssuer = Certificate.Issuer
            }.Matches(Certificate).ShouldBeTrue();
        }

        [Test]
        public void does_not_match_serial_number_matches_certificate_issuer_does_not()
        {
            new SamlCertificate
            {
                SerialNumber = Guid.NewGuid().ToString(),
                CertificateIssuer = Certificate.Issuer
            }.Matches(Certificate).ShouldBeFalse();
        }

        [Test]
        public void does_not_match_certificate_issuer()
        {
            new SamlCertificate
            {
                SerialNumber = Certificate.SerialNumber,
                CertificateIssuer = Guid.NewGuid().ToString()
            }.Matches(Certificate).ShouldBeFalse();
        }

        [Test]
        public void neither_matches()
        {
            new SamlCertificate
            {
                SerialNumber = Guid.NewGuid().ToString(),
                CertificateIssuer = Guid.NewGuid().ToString()
            }.Matches(Certificate).ShouldBeFalse();
        }
    }
}