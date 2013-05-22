using System;
using System.Xml;
using FubuCore;
using FubuTestingSupport;
using NUnit.Framework;
using System.Linq;

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

        [Test]
        public void can_read_the_condition_group_time_constraints()
        {
            theResponse.Conditions.NotBefore.ShouldEqual(XmlConvert.ToDateTimeOffset("2012-11-01T18:13:04Z"));
            theResponse.Conditions.NotOnOrAfter.ShouldEqual(XmlConvert.ToDateTimeOffset("2012-11-01T18:19:04Z"));
        }

        [Test]
        public void can_read_audience_restriction()
        {
            var audienceRestricution = theResponse.Conditions.Conditions
                .Single()
                .ShouldBeOfType<AudienceRestriction>();

            audienceRestricution.Audiences.Single()
                                .ShouldEqual(new Uri("https://qa2.online.com/qa2/sso/saml"));
        }
    }
}