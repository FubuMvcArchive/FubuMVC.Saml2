using System;
using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using FubuCore;
using FubuMVC.Authentication;
using FubuSaml2.Certificates;

namespace FubuMVC.Saml2
{
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