using System;
using FubuCore;
using FubuSaml2.Certificates;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuSaml2.Testing.Certificates
{
    [TestFixture]
    public class BasicSamlCertificateIssuerRepositoryTester
    {
        [Test]
        public void find_by_issuer()
        {
            var issuers = new SamlCertificate[]
            {
                new SamlCertificate{Issuer = new Uri("foo:bar1")}, 
                new SamlCertificate{Issuer = new Uri("foo:bar2")}, 
                new SamlCertificate{Issuer = new Uri("foo:bar3")} 
            };

            var repository = new BasicSamlCertificateRepository(issuers);
            repository.Find(issuers[0].Issuer).ShouldBeTheSameAs(issuers[0]);
            repository.Find(issuers[1].Issuer).ShouldBeTheSameAs(issuers[1]);
            repository.Find(issuers[2].Issuer).ShouldBeTheSameAs(issuers[2]);
        }

        [Test]
        public void find_all_known()
        {
            var issuers = new SamlCertificate[]
            {
                new SamlCertificate{Issuer = new Uri("foo:bar1")}, 
                new SamlCertificate{Issuer = new Uri("foo:bar2")}, 
                new SamlCertificate{Issuer = new Uri("foo:bar3")} 
            };

            var repository = new BasicSamlCertificateRepository(issuers);

            repository.AllKnownCertificates().ShouldHaveTheSameElementsAs(issuers);
        }
    }
}