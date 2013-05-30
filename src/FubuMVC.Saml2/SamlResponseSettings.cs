using FubuMVC.Core.Registration;

namespace FubuMVC.Saml2
{
    [ApplicationLevel]
    public class SamlResponseSettings
    {
        public SamlResponseSettings()
        {
            RequireCertificate = true;
            RequireSignature = true;
            EnforceConditionalTimeSpan = true;
        }

        public bool RequireSignature { get; set; }
        public bool RequireCertificate { get; set; }
        public bool EnforceConditionalTimeSpan { get; set; }
    }
}