using FubuCore;
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

            var samlCertificateRepository = MockRepository.GenerateMock<ISamlCertificateRepository>();
            samlCertificateRepository.Stub(x => x.AllKnownCertificates()).Return(new SamlCertificate[0]);

            registry.Services(x => {
                x.SetServiceIfNone<IPrincipalBuilder>(MockRepository.GenerateMock<IPrincipalBuilder>());
                x.AddService<ISamlResponseHandler>(MockRepository.GenerateMock<ISamlResponseHandler>());
                x.SetServiceIfNone<ISamlCertificateRepository>(samlCertificateRepository);
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

            var samlCertificateRepository = MockRepository.GenerateMock<ISamlCertificateRepository>();
            samlCertificateRepository.Stub(x => x.AllKnownCertificates()).Return(new SamlCertificate[0]);

            registry.Services(x =>
            {
                x.SetServiceIfNone<IPrincipalBuilder>(MockRepository.GenerateMock<IPrincipalBuilder>());
                x.AddService<ISamlResponseHandler>(MockRepository.GenerateMock<ISamlResponseHandler>());
                x.SetServiceIfNone<ISamlCertificateRepository>(samlCertificateRepository);
            });

            registry.AlterSettings<AuthenticationSettings>(x => {
                x.MembershipEnabled = MembershipStatus.Disabled;
            });

            var container = new Container();
            var runtime = FubuApplication.For(registry).StructureMap(container).Bootstrap();


            var strategies = container.GetAllInstances<IAuthenticationStrategy>();
            strategies.Single().ShouldBeOfType<SamlAuthenticationStrategy>();

        }

        [Test]
        public void blows_up_with_no_saml_certificate_repository()
        {
            var registry = new FubuRegistry();

            var samlCertificateRepository = MockRepository.GenerateMock<ISamlCertificateRepository>();
            samlCertificateRepository.Stub(r => r.AllKnownCertificates())
                                     .Return(new SamlCertificate[0]);

            registry.Services(x =>
            {
                x.SetServiceIfNone<IPrincipalBuilder>(MockRepository.GenerateMock<IPrincipalBuilder>());



                x.SetServiceIfNone<ISamlCertificateRepository>(samlCertificateRepository);
                //x.SetServiceIfNone(MockRepository.GenerateMock<ISamlResponseHandler>());

            });

            Exception<FubuException>.ShouldBeThrownBy(() => {
                FubuApplication.For(registry).StructureMap(new Container()).Bootstrap();
            });

            

        }

        [Test]
        public void blows_up_with_no_saml_handlers()
        {
            var registry = new FubuRegistry();

            var samlCertificateRepository = MockRepository.GenerateMock<ISamlCertificateRepository>();
            samlCertificateRepository.Stub(r => r.AllKnownCertificates())
                                     .Return(new SamlCertificate[0]);

            registry.Services(x =>
            {
                x.SetServiceIfNone<IPrincipalBuilder>(MockRepository.GenerateMock<IPrincipalBuilder>());



                x.SetServiceIfNone<ISamlCertificateRepository>(samlCertificateRepository);
                //x.SetServiceIfNone(MockRepository.GenerateMock<ISamlResponseHandler>());

            });

            Exception<FubuException>.ShouldBeThrownBy(() =>
            {
                FubuApplication.For(registry).StructureMap(new Container()).Bootstrap();
            });



        }
    }
}