using FubuSaml2.Validation;
using NUnit.Framework;
using FubuTestingSupport;

namespace FubuSaml2.Testing
{
    [TestFixture]
    public class SamlErrorTester
    {
        [Test]
        public void build_by_string_token()
        {
            var error = new SamlError(SamlValidationKeys.TimeFrameDoesNotMatch);
            error.Key.ShouldEqual("TimeFrameDoesNotMatch");
            error.Message.ShouldEqual(SamlValidationKeys.TimeFrameDoesNotMatch.ToString());
        }
    }
}