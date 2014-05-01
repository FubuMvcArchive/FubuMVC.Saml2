using FubuMVC.Authentication;
using FubuMVC.Core;
using FubuMVC.Core.Registration;

namespace FubuMVC.Saml2
{
    public class Saml2AuthenticationRegistration : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Settings.Get<AuthenticationSettings>()
                 .Strategies.InsertFirst(new AuthenticationNode(typeof(SamlAuthenticationStrategy)));
        }
    }
}