using System;
using System.Xml;
using FubuCore;
using FubuSaml2.Xml;
using FubuTestingSupport;
using NUnit.Framework;
using System.Linq;

namespace FubuSaml2.Testing.Xml
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
        public void reads_the_destination()
        {
            theResponse.Destination.ShouldEqual("https://qa2.online.com/qa2/sso/saml".ToUri());
        }

        [Test]
        public void reads_the_id()
        {
            theResponse.Id.ShouldEqual("A5092bc640a235880200023f80002aa33");
        }

        [Test]
        public void reads_the_issue_instant()
        {
            theResponse.IssueInstant.ShouldEqual(XmlConvert.ToDateTimeOffset("2012-11-01T18:16:04Z"));
        }

        [Test]
        public void read_the_issuer()
        {
            theResponse.Issuer.ShouldEqual(new Uri("urn:idp:fakecompany:nbpartgenoutbds20:uat"));
        }

        [Test]
        public void read_the_status_of_the_response_if_it_is_success()
        {
            theResponse.Status.ShouldEqual(SamlStatus.Success);
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
                Value = "aa50045c6d0a233e7c20003d7d0000aa33",
                Format = NameFormat.Persistent
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

        [Test]
        public void reads_the_basic_authentication_properties()
        {
            theResponse.Authentication.Instant.ShouldEqual(XmlConvert.ToDateTimeOffset("2012-11-01T18:15:47Z"));
            theResponse.Authentication.SessionIndex.ShouldEqual("21171a497ffc16b87b8179afd5f9b69fc3f8cd8c");
            theResponse.Authentication.SessionNotOnOrAfter.ShouldEqual(XmlConvert.ToDateTimeOffset("2012-11-02T04:15:47Z"));

        }

        [Test]
        public void reads_the_authentication_context()
        {
            theResponse.Authentication.DeclarationReference
                       .ShouldEqual("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport".ToUri());
        }
    }
}