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
            SamlStatus.ToSamlStatus(SamlStatus.Success.Uri.ToString()).ShouldBeTheSameAs(SamlStatus.Success);
            SamlStatus.ToSamlStatus(SamlStatus.RequesterError.Uri.ToString()).ShouldBeTheSameAs(SamlStatus.RequesterError);
            SamlStatus.ToSamlStatus(SamlStatus.ResponderError.Uri.ToString()).ShouldBeTheSameAs(SamlStatus.ResponderError);
        }
    }
}