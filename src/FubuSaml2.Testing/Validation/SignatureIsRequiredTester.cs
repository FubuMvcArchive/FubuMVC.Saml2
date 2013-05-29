using FubuSaml2.Validation;
using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;

namespace FubuSaml2.Testing.Validation
{
    [TestFixture]
    public class SignatureIsRequiredTester
    {
        [Test]
        public void no_errors_if_response_is_signed()
        {
            var response = new SamlResponse
            {
                Signed = SignatureStatus.Signed
            };

            new SignatureIsRequired().Validate(response);

            response.Errors.Any().ShouldBeFalse();
        }

        [Test]
        public void error_if_signature_is_missing()
        {
            var response = new SamlResponse
            {
                Signed = SignatureStatus.NotSigned
            };

            new SignatureIsRequired().Validate(response);

            response.Errors.Single()
                    .ShouldEqual(new SamlError(SignatureStatus.NotSigned));
        }

        [Test]
        public void error_if_signature_is_invalid()
        {
            var response = new SamlResponse
            {
                Signed = SignatureStatus.InvalidSignature
            };

            new SignatureIsRequired().Validate(response);

            response.Errors.Single()
                    .ShouldEqual(new SamlError(SignatureStatus.InvalidSignature));
        }
    }
}