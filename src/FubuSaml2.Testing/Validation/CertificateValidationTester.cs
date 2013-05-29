using FubuSaml2.Certificates;
using FubuSaml2.Validation;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;

namespace FubuSaml2.Testing.Validation
{
    [TestFixture]
    public class CertificateValidationTester : InteractionContext<CertificateValidation>
    {
        private SamlResponse response;

        protected override void beforeEach()
        {
            response = new SamlResponse {Certificates = new ICertificate[] {ObjectMother.Certificate1()}};
        }

        private SamlValidationKeys theCertificateValidationReturns
        {
            set
            {
                MockFor<ICertificateService>().Stub(x => x.Validate(response))
                                              .Return(value);
            }
        }

        [Test]
        public void logs_no_error_if_the_certificate_is_valid()
        {

            theCertificateValidationReturns = SamlValidationKeys.ValidCertificate;

            ClassUnderTest.Validate(response);

            response.Errors.Any().ShouldBeFalse();
        }

        [Test]
        public void logs_error_if_certificate_does_not_match_issuer()
        {
            theCertificateValidationReturns = SamlValidationKeys.CannotMatchIssuer;
            ClassUnderTest.Validate(response);

            response.Errors.Single().ShouldEqual(new SamlError(SamlValidationKeys.CannotMatchIssuer));
        }

        [Test]
        public void logs_error_if_certificate_is_invalid()
        {
            theCertificateValidationReturns = SamlValidationKeys.NoValidCertificates;
            ClassUnderTest.Validate(response);

            response.Errors.Single().ShouldEqual(new SamlError(SamlValidationKeys.NoValidCertificates));
        }
    }
}