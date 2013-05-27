using NUnit.Framework;
using FubuTestingSupport;

namespace FubuSaml2.Testing
{
    [TestFixture]
    public class SamlStatusTester
    {
        [Test]
        public void can_read_saml_status()
        {
            SamlStatus.Get(SamlStatus.Success.Uri.ToString()).ShouldBeTheSameAs(SamlStatus.Success);
            SamlStatus.Get(SamlStatus.RequesterError.Uri.ToString()).ShouldBeTheSameAs(SamlStatus.RequesterError);
            SamlStatus.Get(SamlStatus.ResponderError.Uri.ToString()).ShouldBeTheSameAs(SamlStatus.ResponderError);
        }
    }
}