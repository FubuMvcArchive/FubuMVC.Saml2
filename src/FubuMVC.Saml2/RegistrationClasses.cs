using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
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

            // more probably
        }
    }

    [ConfigurationType(ConfigurationType.Explicit)]
    public class Saml2AuthenticationRegistration : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            // TODO -- put the SamlAuthenticationRegistry first
            // TODO -- test with and without basic auth disabled
            throw new NotImplementedException();
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
            // mark failure if not all the certificates can be loaded
            // mark failure if ISamlCertificateRepository is not loaded
            // mark failure if no ISamlResponseHandler's are registered
        }
    }
}