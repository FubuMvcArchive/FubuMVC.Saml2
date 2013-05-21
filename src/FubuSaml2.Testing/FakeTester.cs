using NUnit.Framework;
using FubuTestingSupport;

namespace FubuSaml2.Testing
{
    [TestFixture]
    public class FakeTester
    {
        [Test]
        public void placeholder()
        {
            true.ShouldBeTrue();
        }
    }
}