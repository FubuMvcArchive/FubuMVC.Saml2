using FubuMVC.Authentication;
using FubuMVC.Authentication.Membership;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuSaml2.Certificates;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap;
using FubuMVC.StructureMap;
using System.Linq;
using FubuTestingSupport;

namespace FubuMVC.Saml2.Testing
{
    [TestFixture]
    public class bottle_configuration_integration_tester
    {
        [Test]
        public void register_with_basic_authentication_enabled()
        {
            var registry = new FubuRegistry();
            registry.Import<Saml2Extensions>();
            registry.Import<ApplyAuthentication>();
            registry.Services(x => {
                x.SetServiceIfNone<IPrincipalBuilder>(MockRepository.GenerateMock<IPrincipalBuilder>());
                x.SetServiceIfNone<ISamlCertificateRepository>(MockRepository.GenerateMock<ISamlCertificateRepository>());
            });

            var container = new Container();
            var runtime = FubuApplication.For(registry).StructureMap(container).Bootstrap();


            var strategies = container.GetAllInstances<IAuthenticationStrategy>();
            strategies.First().ShouldBeOfType<SamlAuthenticationStrategy>();

            strategies.Last().ShouldBeOfType<MembershipAuthentication>();
        }

        [Test]
        public void register_with_basic_authentication_disabled()
        {
            var registry = new FubuRegistry();
            registry.Import<Saml2Extensions>();
            registry.Import<ApplyAuthentication>();
            registry.Services(x =>
            {
                x.SetServiceIfNone<IPrincipalBuilder>(MockRepository.GenerateMock<IPrincipalBuilder>());
                x.SetServiceIfNone<ISamlCertificateRepository>(MockRepository.GenerateMock<ISamlCertificateRepository>());
            });

            registry.AlterSettings<AuthenticationSettings>(x => {
                x.MembershipEnabled = MembershipStatus.Disabled;
            });

            var container = new Container();
            var runtime = FubuApplication.For(registry).StructureMap(container).Bootstrap();


            var strategies = container.GetAllInstances<IAuthenticationStrategy>();
            strategies.Single().ShouldBeOfType<SamlAuthenticationStrategy>();

        }
    }
}