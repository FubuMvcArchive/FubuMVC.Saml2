using FubuMVC.Core.Registration;
using FubuSaml2.Certificates;
using FubuSaml2.Encryption;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Saml2.Testing
{
    [TestFixture]
    public class Saml2ServicesRegistry_specification
    {
        private void registeredTypeIs<TService, TImplementation>()
        {
            BehaviorGraph.BuildFrom(x => {
                x.Services<Saml2ServicesRegistry>();
            }).Services.DefaultServiceFor<TService>().Type.ShouldEqual(
                typeof(TImplementation));
        }

        [Test]
        public void ISamlDirector()
        {
            registeredTypeIs<ISamlDirector, SamlDirector>();
        }

        [Test]
        public void ISamlResponseReader()
        {
            registeredTypeIs<ISamlResponseReader, SamlResponseReader>();
        }

        [Test]
        public void ISamlResponseWriter()
        {
            registeredTypeIs<ISamlResponseWriter, SamlResponseWriter>();
        }

        [Test]
        public void ICertificateService()
        {
            registeredTypeIs<ICertificateService, CertificateService>();
        }

        [Test]
        public void ICertificateLoader()
        {
            registeredTypeIs<ICertificateLoader, CertificateLoader>();
        }

        [Test]
        public void IAssertionXmlDecryptor()
        {
            registeredTypeIs<IAssertionXmlDecryptor, AssertionXmlDecryptor>();
        }
    }
}