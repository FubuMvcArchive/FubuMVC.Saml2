using System;
using FubuCore;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuSaml2.Testing
{
    [TestFixture]
    public class SamlResponseXmlReaderTester
    {
        private SamlResponseXmlReader theReader;

        [SetUp]
        public void SetUp()
        {
            var xml = new FileSystem().ReadStringFromFile("sample.xml");
            theReader = new SamlResponseXmlReader(xml);
        }

        [Test]
        public void read_the_issuer()
        {
            theReader.Issuer.ShouldEqual(new Uri("urn:idp:fidelity:nbpartgenoutbds20:uat"));
        }
    }
}