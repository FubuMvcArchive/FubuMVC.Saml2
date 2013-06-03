using Bottles;
using FubuMVC.Core.Registration;
using FubuSaml2.Certificates;
using FubuSaml2.Encryption;

namespace FubuMVC.Saml2
{
    public class Saml2ServicesRegistry : ServiceRegistry
    {
        public Saml2ServicesRegistry()
        {
            // TODO -- UT's these
            SetServiceIfNone<ISamlDirector, SamlDirector>();
            SetServiceIfNone<ISamlResponseReader, SamlResponseReader>();
            SetServiceIfNone<ISamlResponseWriter, SamlResponseWriter>();
            SetServiceIfNone<ICertificateService, CertificateService>();
            SetServiceIfNone<IAssertionXmlDecryptor, AssertionXmlDecryptor>();
            SetServiceIfNone<ICertificateLoader, CertificateLoader>();

            AddService<IActivator, Saml2VerificationActivator>();
        }
    }
}