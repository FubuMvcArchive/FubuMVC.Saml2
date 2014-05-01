using FubuMVC.Core.Registration;
using FubuSaml2.Validation;

namespace FubuMVC.Saml2
{
    public class SamlResponseValidationRulesRegistration : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            var settings = graph.Settings.Get<SamlResponseSettings>();

            if (settings.RequireSignature)
            {
                graph.Services.AddService<ISamlValidationRule, SignatureIsRequired>();
            }

            if (settings.RequireCertificate)
            {
                graph.Services.AddService<ISamlValidationRule, CertificateValidation>();
            }

            if (settings.EnforceConditionalTimeSpan)
            {
                graph.Services.AddService<ISamlValidationRule, ConditionTimeFrame>();
            }
        }
    }
}