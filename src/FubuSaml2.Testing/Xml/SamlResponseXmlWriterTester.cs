using System.Diagnostics;
using System.Xml;
using FubuCore;
using FubuSaml2.Xml;
using NUnit.Framework;
using FubuTestingSupport;
using System.Linq;

namespace FubuSaml2.Testing.Xml 
{
    [TestFixture]
    public class SamlResponseXmlWriterTester
    {
        private SamlResponse theOriginalResponse;
        private XmlDocument document;
        private SamlResponse theSecondResponse;

        // TODO -- try to verify this against the real spec XSD

        [TestFixtureSetUp]
        public void SetUp()
        {
            var xml = new FileSystem().ReadStringFromFile("sample.xml");
            theOriginalResponse = new SamlResponseXmlReader(xml).Read();

            document = new SamlResponseXmlWriter(theOriginalResponse).Write();
            
            Debug.WriteLine(document.OuterXml);
            
            theSecondResponse = new SamlResponseXmlReader(document.OuterXml).Read();
        }

        [Test]
        public void writes_the_assertion_element()
        {
            var assertion = document.DocumentElement.FindChild("Assertion");
            assertion.ShouldNotBeNull();
            assertion.GetAttribute("ID").ShouldEqual("A5092bc640a235880200023f80002aa33");
            assertion.GetAttribute("IssueInstant").ShouldEqual("2012-11-01T18:16:04Z");

            var issuer = assertion.FirstChild;
            issuer.Name.ShouldEqual("Issuer");
            issuer.InnerText.ShouldEqual("urn:idp:fakecompany:nbpartgenoutbds20:uat");
        }

        [Test]
        public void version_should_be_2_point_0()
        {
            document.DocumentElement.GetAttribute("Version").ShouldEqual("2.0");
        }

        [Test]
        public void writes_the_issuer()
        {
            theSecondResponse.Issuer.ShouldEqual(theOriginalResponse.Issuer);
        }

        [Test]
        public void writes_the_status()
        {
            theSecondResponse.Status.ShouldEqual(theOriginalResponse.Status);
        }

        [Test]
        public void writes_the_Destination()
        {
            theSecondResponse.Destination.ShouldEqual(theOriginalResponse.Destination);
        }

        [Test]
        public void writes_the_Id()
        {
            theSecondResponse.Id.ShouldEqual(theOriginalResponse.Id);
        }

        [Test]
        public void writes_the_issue_instant()
        {
            theSecondResponse.IssueInstant.ShouldEqual(theOriginalResponse.IssueInstant);
        }

        [Test]
        public void writes_the_subject_name()
        {
            theSecondResponse.Subject.Name.ShouldEqual(theOriginalResponse.Subject.Name);
        }

        [Test]
        public void writes_the_subject_format()
        {
            theSecondResponse.Subject.Name.Format
                             .ShouldEqual(theOriginalResponse.Subject.Name.Format);
        }

        [Test]
        public void writes_the_subject_confirmation_methods()
        {
            theSecondResponse.Subject.Confirmations.Select(x => x.Method)
                .ShouldHaveTheSameElementsAs(theOriginalResponse.Subject.Confirmations.Select(x => x.Method));

        }
        
        [Test]
        public void writes_the_subject_confirmation_name()
        {
            theSecondResponse.Subject.Confirmations.Select(x => x.Name)
                .ShouldHaveTheSameElementsAs(theOriginalResponse.Subject.Confirmations.Select(x => x.Name));

        }

        [Test]
        public void writes_the_subject_confirmation_data()
        {
            var secondConfirmationData = theSecondResponse.Subject.Confirmations.First().ConfirmationData.First();
            var originalConfirmationData = theOriginalResponse.Subject.Confirmations.First().ConfirmationData.First();

            secondConfirmationData.NotOnOrAfter.ShouldEqual(originalConfirmationData.NotOnOrAfter);
            secondConfirmationData.Recipient.ShouldEqual(originalConfirmationData.Recipient);
        }

        [Test]
        public void writes_the_condition_group_times()
        {
            theSecondResponse.Conditions.NotBefore
                             .ShouldEqual(theOriginalResponse.Conditions.NotBefore);

            theSecondResponse.Conditions.NotOnOrAfter
                             .ShouldEqual(theOriginalResponse.Conditions.NotOnOrAfter);
        }

        [Test]
        public void writes_the_audiences()
        {
            var secondAudiences = theSecondResponse.Conditions.Conditions.OfType<AudienceRestriction>().Select(x => x.Audiences);
            var originalAudiences =
                theOriginalResponse.Conditions.Conditions.OfType<AudienceRestriction>().Select(x => x.Audiences);


            secondAudiences.ShouldHaveTheSameElementsAs(originalAudiences);
        }

        [Test]
        public void writes_the_authentication_context_basic_properties()
        {
            theSecondResponse.Authentication.Instant.ShouldEqual(theOriginalResponse.Authentication.Instant);
            theSecondResponse.Authentication.SessionIndex.ShouldEqual(theOriginalResponse.Authentication.SessionIndex);
            theSecondResponse.Authentication.SessionNotOnOrAfter.ShouldEqual(theOriginalResponse.Authentication.SessionNotOnOrAfter);
        }

        [Test]
        public void writes_the_authentication_context_declaration_reference()
        {
            theSecondResponse.Authentication.DeclarationReference
                             .ShouldEqual(theOriginalResponse.Authentication.DeclarationReference);
        }

        [Test]
        public void writes_the_attributes()
        {
            theSecondResponse.Attributes.Get("ClientId")
                             .ShouldEqual("000012345");

            theSecondResponse.Attributes.Get("CustomerId")
                             .ShouldEqual("001010111");
        }
    }
}