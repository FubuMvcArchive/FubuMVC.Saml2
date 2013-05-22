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
        private SamlResponse theResponse;

        [SetUp]
        public void SetUp()
        {
            var xml = new FileSystem().ReadStringFromFile("sample.xml");
            theReader = new SamlResponseXmlReader(xml);

            theResponse = theReader.Read();
        }

        [Test]
        public void read_the_issuer()
        {
            theResponse.Issuer.ShouldEqual(new Uri("urn:idp:fidelity:nbpartgenoutbds20:uat"));
        }

        [Test]
        public void read_the_status_of_the_response_if_it_is_success()
        {
            theResponse.Status.ShouldEqual(SamlResponseStatus.Success);
        }
    }
}