using FubuMVC.Core.Registration;
using NUnit.Framework;
using FubuTestingSupport;
using FubuCore.Reflection;

namespace FubuMVC.Saml2.Testing
{
    [TestFixture]
    public class SamlResponseSettingsTester
    {
        [Test]
        public void require_certificate_is_true_by_default()
        {
            new SamlResponseSettings().RequireCertificate.ShouldBeTrue();
        }

        [Test]
        public void require_signature_is_true_by_default()
        {
            new SamlResponseSettings().RequireCertificate.ShouldBeTrue();
        }

        [Test]
        public void enforce_response_time_span_is_true_y_default()
        {
            new SamlResponseSettings().EnforceConditionalTimeSpan.ShouldBeTrue();
        }

        [Test]
        public void settings_is_marked_as_application_level()
        {
            typeof (SamlResponseSettings).HasAttribute<ApplicationLevelAttribute>();
        }
    }
}