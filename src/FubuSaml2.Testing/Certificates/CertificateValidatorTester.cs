using System;
using FubuSaml2.Certificates;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;

namespace FubuSaml2.Testing.Certificates    
{
    [TestFixture]
    public class CertificateValidatorTester : InteractionContext<CertificateValidator>
    {
        [Test]
        public void matches_issuer_only_has_to_match_once_single_cert()
        {
            var cert = ObjectMother.Certificate1();
            var response = new SamlResponse
            {
                Issuer = new Uri("this:guy"),
                Certificates = new ICertificate[] {cert}
            };

            var matchers = Services.CreateMockArrayFor<ICertificateIssuerMatcher>(4);
            matchers[3].Stub(x => x.MatchesIssuer(response.Issuer, cert))
                       .Return(true);

            ClassUnderTest.MatchesIssuer(response)
                .ShouldBeTrue();
        }

        [Test]
        public void does_not_match_issuer_if_all_strategies_fail()
        {
            var cert = ObjectMother.Certificate1();
            var response = new SamlResponse
            {
                Issuer = new Uri("this:guy"),
                Certificates = new ICertificate[] { cert }
            };

            // The MatchesIssuer method will return false by default unless 
            // you stub it to something else
            var matchers = Services.CreateMockArrayFor<ICertificateIssuerMatcher>(4);

            ClassUnderTest.MatchesIssuer(response)
                .ShouldBeFalse();
        }

        [Test]
        public void only_has_to_match_on_one_cert()
        {
            var certs = new ICertificate[]
            {
                new InMemoryCertificate(),
                new InMemoryCertificate(),
                new InMemoryCertificate(),
                new InMemoryCertificate()
            };

            var response = new SamlResponse
            {
                Issuer = new Uri("this:guy"),
                Certificates = certs
            };

            var matchers = Services.CreateMockArrayFor<ICertificateIssuerMatcher>(4);
            matchers[3].Stub(x => x.MatchesIssuer(response.Issuer, certs.Last()))
                       .Return(true);

            ClassUnderTest.MatchesIssuer(response)
                .ShouldBeTrue();
        }

        [Test]
        public void fails_if_no_certs_match_any_of_the_matchers()
        {
            var certs = new ICertificate[]
            {
                new InMemoryCertificate(),
                new InMemoryCertificate(),
                new InMemoryCertificate(),
                new InMemoryCertificate()
            };

            var response = new SamlResponse
            {
                Issuer = new Uri("this:guy"),
                Certificates = certs
            };

            var matchers = Services.CreateMockArrayFor<ICertificateIssuerMatcher>(4);

            ClassUnderTest.MatchesIssuer(response)
                .ShouldBeFalse();
        }
    }
}