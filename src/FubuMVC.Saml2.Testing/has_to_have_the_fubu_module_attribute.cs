using System.Reflection;
using FubuMVC.Core;
using NUnit.Framework;
using System.Linq;
using FubuTestingSupport;

namespace FubuMVC.Saml2.Testing
{
    [TestFixture]
    public class has_to_have_the_fubu_module_attribute
    {
        [Test]
        public void has_to_have_the_attribute()
        {
            Assembly.Load("FubuMVC.Saml2").GetCustomAttributes(typeof(FubuModuleAttribute), false)
                .Any().ShouldBeTrue();

        }
    }
}