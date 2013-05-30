using FubuMVC.Core.Registration;
using FubuSaml2.Validation;
using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;

namespace FubuMVC.Saml2.Testing
{
    [TestFixture]
    public class SamlResponseValidationRulesRegistrationTester
    {
        [Test]
        public void require_signatures_is_true()
        {
            var graph = new BehaviorGraph();
            graph.Settings.Get<SamlResponseSettings>().RequireSignature = true;

            new SamlResponseValidationRulesRegistration().Configure(graph);

            graph.Services.ServicesFor<ISamlValidationRule>().Select(x => x.Type)
                .ShouldContain(typeof(SignatureIsRequired));
        }

        [Test]
        public void require_signatures_is_false()
        {
            var graph = new BehaviorGraph();
            graph.Settings.Get<SamlResponseSettings>().RequireSignature = false;

            new SamlResponseValidationRulesRegistration().Configure(graph);

            graph.Services.ServicesFor<ISamlValidationRule>().Select(x => x.Type)
                .ShouldNotContain(typeof(SignatureIsRequired));
        }

        [Test]
        public void require_certificate_is_true()
        {
            var graph = new BehaviorGraph();
            graph.Settings.Get<SamlResponseSettings>().RequireCertificate = true;

            new SamlResponseValidationRulesRegistration().Configure(graph);

            graph.Services.ServicesFor<ISamlValidationRule>().Select(x => x.Type)
                .ShouldContain(typeof(CertificateValidation));
        }

        [Test]
        public void require_certificate_is_false()
        {
            var graph = new BehaviorGraph();
            graph.Settings.Get<SamlResponseSettings>().RequireCertificate = false;

            new SamlResponseValidationRulesRegistration().Configure(graph);

            graph.Services.ServicesFor<ISamlValidationRule>().Select(x => x.Type)
                .ShouldNotContain(typeof(CertificateValidation));
        }

        [Test]
        public void enforce_response_timespan_is_true()
        {
            var graph = new BehaviorGraph();
            graph.Settings.Get<SamlResponseSettings>().EnforceConditionalTimeSpan = true;

            new SamlResponseValidationRulesRegistration().Configure(graph);

            graph.Services.ServicesFor<ISamlValidationRule>().Select(x => x.Type)
                .ShouldContain(typeof(ConditionTimeFrame));
        }

        [Test]
        public void require_conditional_timespan_is_false()
        {
            var graph = new BehaviorGraph();
            graph.Settings.Get<SamlResponseSettings>().EnforceConditionalTimeSpan = false;

            new SamlResponseValidationRulesRegistration().Configure(graph);

            graph.Services.ServicesFor<ISamlValidationRule>().Select(x => x.Type)
                .ShouldNotContain(typeof(ConditionTimeFrame));
        }
    }
}