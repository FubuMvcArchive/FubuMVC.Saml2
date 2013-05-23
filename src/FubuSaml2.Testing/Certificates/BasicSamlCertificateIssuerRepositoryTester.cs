using System;
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
            var issuers = new SamlCertificateIssuer[]
            {
                new SamlCertificateIssuer{SamlIssuer = new Uri("foo:bar1")}, 
                new SamlCertificateIssuer{SamlIssuer = new Uri("foo:bar2")}, 
                new SamlCertificateIssuer{SamlIssuer = new Uri("foo:bar3")} 
            };

            var repository = new BasicSamlCertificateIssuerRepository(issuers);
            repository.Find(issuers[0].SamlIssuer).ShouldBeTheSameAs(issuers[0]);
            repository.Find(issuers[1].SamlIssuer).ShouldBeTheSameAs(issuers[1]);
            repository.Find(issuers[2].SamlIssuer).ShouldBeTheSameAs(issuers[2]);
        }
    }
}