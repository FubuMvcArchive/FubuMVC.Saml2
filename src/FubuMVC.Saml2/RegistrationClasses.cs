using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
using FubuMVC.Authentication;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuSaml2.Certificates;
using FubuSaml2.Encryption;

namespace FubuMVC.Saml2
{
    public class Saml2Extensions : IFubuRegistryExtension
    {
        public void Configure(FubuRegistry registry)
        {
            registry.Policies.Add<SamlResponseValidationRulesRegistration>();
            registry.Policies.Add<Saml2AuthenticationRegistration>();
            registry.Services<Saml2ServicesRegistry>();


        }
    }

    public class Saml2ServicesRegistry : ServiceRegistry
    {
        public Saml2ServicesRegistry()
        {
            // TODO -- UT's these
            SetServiceIfNone<ISamlDirector, SamlDirector>();
            SetServiceIfNone<ISamlResponseReader, SamlResponseReader>();
            SetServiceIfNone<ICertificateService, CertificateService>();
            SetServiceIfNone<IAssertionXmlDecryptor, AssertionXmlDecryptor>();
            SetServiceIfNone<ICertificateLoader, CertificateLoader>();

            AddService<IActivator, Saml2VerificationActivator>();
        }
    }

    [ConfigurationType(ConfigurationType.Explicit)]
    public class Saml2AuthenticationRegistration : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Settings.Get<AuthenticationSettings>()
                .Strategies.InsertFirst(new AuthenticationNode(typeof(SamlAuthenticationStrategy)));
        }
    }

    public class Saml2VerificationActivator : IActivator
    {
        private readonly IServiceLocator _services;

        public Saml2VerificationActivator(IServiceLocator services)
        {
            _services = services;

        }


        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            var repository = _services.VerifyRegistration<ISamlCertificateRepository>(log);
            _services.VerifyRegistration<IPrincipalBuilder>(log);
            _services.VerifyAnyRegistrations<ISamlResponseHandler>(log);
 
            if (repository != null)
            {
                checkCertificates(repository, log);
            }
        }

        private void checkCertificates(ISamlCertificateRepository repository, IPackageLog log)
        {
            var loader = _services.GetInstance<ICertificateLoader>();

            repository.AllKnownCertificates().Each(samlCertificate => {
                try
                {
                    var certificate = loader.Load(samlCertificate.Thumbprint);
                    if (certificate == null)
                    {
                        log.MarkFailure("Could not load Certificate for Issuer " + samlCertificate.Issuer);
                    }
                }
                catch (Exception ex)
                {
                    log.MarkFailure("Could not load Certificate for Issuer " + samlCertificate.Issuer);
                    log.MarkFailure(ex);
                }
            });
        }
    }
}








