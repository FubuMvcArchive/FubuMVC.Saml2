using System;
using System.Xml;
using FubuCore;
using FubuSaml2.Xml;
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

        [Test]
        public void can_read_the_subject_name()
        {
            theResponse.Subject.Name.ShouldEqual(new SamlName
            {
                Type = SamlNameType.NameID,
                Value = "aa50045c6d0a233e7c20003d7d0000aa33"
            });
        }

        [Test]
        public void can_read_the_subject_confirmation_method()
        {
            theResponse.Subject.Confirmations.Single()
                       .Method.ShouldEqual("urn:oasis:names:tc:SAML:2.0:cm:bearer".ToUri());
        }

        [Test]
        public void subject_confirmation_data()
        {
            var data = theResponse.Subject.Confirmations.Single()
                                  .ConfirmationData.Single();

            data.NotOnOrAfter.ShouldEqual(XmlConvert.ToDateTimeOffset("2012-11-01T18:19:04Z"));
            data.Recipient.ShouldEqual("https://qa2.online.com/qa2/sso/saml".ToUri());

        }

        [Test]
        public void has_the_attributes()
        {
            theResponse.Attributes.Get("ClientId").ShouldEqual("000012345");
            theResponse.Attributes.Get("CustomerId").ShouldEqual("001010111");
        }

        [Test]
        public void can_read_the_certificates()
        {
            theResponse.Certificates.Any().ShouldBeTrue();
        }

        [Test, Ignore("CANNOT USE THIS UNTIL WE GET FAKE DATA")]
        public void is_signed_positive()
        {
            theResponse.Signed.ShouldEqual(SignatureStatus.Signed);
        }
    }
}