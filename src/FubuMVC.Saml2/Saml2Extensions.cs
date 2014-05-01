using FubuMVC.Core;

namespace FubuMVC.Saml2
{
    public class Saml2Extensions : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Global.Add<SamlResponseValidationRulesRegistration>();
            registry.Policies.Global.Add<Saml2AuthenticationRegistration>();
            registry.Services<Saml2ServicesRegistry>();


        }
    }
}








