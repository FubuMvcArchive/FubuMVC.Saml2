using System.Collections.Generic;
using FubuTestingSupport;
using NUnit.Framework;
using FubuCore;

namespace FubuSaml2.Testing
{
    [TestFixture]
    public class SamlResponseTester
    {
        [Test]
        public void add_a_single_key_attribute()
        {
            var response = new SamlResponse();
            response.AddAttribute("a", "1");

            response.Attributes.Get("a").ShouldEqual("1");
        }

        [Test]
        public void add_multiple_values_for_the_same_key()
        {
            var response = new SamlResponse();

            response.AddAttribute("a", "1");
            response.AddAttribute("a", "2");
            response.AddAttribute("a", "3");

            response.Attributes.Get("a").As<IEnumerable<string>>()
                .ShouldHaveTheSameElementsAs("1", "2", "3");
        }
    }
}