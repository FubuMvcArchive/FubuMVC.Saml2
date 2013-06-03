using System;
using Bottles.Diagnostics;
using FubuCore;
using FubuSaml2.Certificates;
using NUnit.Framework;
using Rhino.Mocks;
using FubuTestingSupport;

namespace FubuMVC.Saml2.Testing
{
    [TestFixture]
    public class EnvironmentTestExtensionsTester
    {
        private IServiceLocator theServices;
        private IPackageLog theLog;

        [SetUp]
        public void SetUp()
        {
            theServices = MockRepository.GenerateMock<IServiceLocator>();
            theLog = new PackageLog();
        }

        [Test]
        public void verify_test_extensions_everything_happy()
        {
            var theFoo = new Foo();
            theServices.Stub(x => x.GetInstance<IFoo>())
                       .Return(theFoo);

            theServices.VerifyRegistration<IFoo>(theLog)
                .ShouldBeTheSameAs(theFoo);

            theLog.Success.ShouldBeTrue();
            theLog.FullTraceText().ShouldContain("Using {0} for {1}".ToFormat(typeof(Foo).FullName, typeof(IFoo).FullName));
        }

        [Test]
        public void verify_test_extensions_failure()
        {
            var exception = new NotImplementedException();
            theServices.Stub(x => x.GetInstance<IFoo>()).Throw(exception);

            theServices.VerifyRegistration<IFoo>(theLog).ShouldBeNull();

            theLog.Success.ShouldBeFalse();
            theLog.FullTraceText().ShouldContain(exception.ToString());
            theLog.FullTraceText().ShouldContain("Could not resolve " + typeof(IFoo).FullName);
        }
    }

    public interface IFoo
    {
        
    }

    public class Foo : IFoo{}
}