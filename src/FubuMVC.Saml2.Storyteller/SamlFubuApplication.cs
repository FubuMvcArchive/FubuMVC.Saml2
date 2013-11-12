using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using FubuMVC.Authentication;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuSaml2.Certificates;
using FubuSaml2.Xml;
using StructureMap;
using FubuCore;

namespace FubuMVC.Saml2.Storyteller
{
    public class SamlFubuApplication : IApplicationSource
    {
        public readonly static SamlCertificate SamlCertificate;
        public readonly static X509Certificate2 Certificate;


        static SamlFubuApplication()
        {
            var location = AppDomain.CurrentDomain.BaseDirectory;

            var certPath = location.AppendPath("cert2.pfx");

            if (!File.Exists(certPath))
            {
                throw new InvalidOperationException("Couldn't find path " + certPath);
            }

            var cert = new X509Certificate2(certPath, new SecureString(), X509KeyStorageFlags.Exportable);
            Certificate = new X509Certificate2(cert);

            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            store.Add(Certificate);
            

            SamlCertificate = new SamlCertificate
            {
                Issuer = "fake:saml:issuer",
                CertificateIssuer = Certificate.Issuer,
                SerialNumber = Certificate.SerialNumber,
                Thumbprint = Certificate.Thumbprint
            };
        }

        public FubuApplication BuildApplication()
        {
            var repository = new BasicSamlCertificateRepository(new SamlCertificate[] {SamlCertificate});
            var container = new Container(x => {
                x.For<IPrincipalBuilder>().Use<SimplePrincipalBuilder>();
                x.For<ISamlCertificateRepository>().Use(repository);

                x.For<ISamlResponseHandler>().Use<FakeSamlResponseHandler>();
                x.For<ICertificateLoader>().Use(new InMemoryCertificateLoader(Certificate));
            });

            

            return FubuApplication.For<SamlStorytellerFubuRegistry>().StructureMap(container);
        }
    }

    public class SamlStorytellerFubuRegistry : FubuRegistry
    {
        public SamlStorytellerFubuRegistry()
        {
            Services(x => x.ReplaceService<ICertificateLoader>(new InMemoryCertificateLoader(SamlFubuApplication.Certificate)));
        }
    }


    public class SimplePrincipalBuilder : IPrincipalBuilder
    {
        public IPrincipal Build(string userName)
        {
            var identity = new GenericIdentity(userName);
            return new GenericPrincipal(identity, new string[0]);
        }
    }
}