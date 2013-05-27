using NUnit.Framework;
using FubuTestingSupport;

namespace FubuSaml2.Testing
{
    [TestFixture]
    public class SamlNameTester
    {
        [Test]
        public void NameID_by_default()
        {
            new SamlName().Type.ShouldEqual(SamlNameType.NameID);
        }

        [Test]
        public void format_is_unspecified_by_default()
        {
            new SamlName().Format.ShouldEqual(NameFormat.Unspecified);
        }

        [Test]
        public void name_format_can_find_itself()
        {
            NameFormat.Get(NameFormat.Persistent.Uri.ToString())
                      .ShouldBeTheSameAs(NameFormat.Persistent);
        }
    }
}