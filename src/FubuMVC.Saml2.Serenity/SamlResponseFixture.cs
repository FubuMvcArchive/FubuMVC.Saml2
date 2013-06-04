using System;
using System.ComponentModel;
using FubuCore.Dates;
using FubuSaml2;
using FubuSaml2.Certificates;
using OpenQA.Selenium;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Engine;
using FubuSaml2.Xml;

namespace FubuMVC.Saml2.Serenity
{
    public class SamlResponseFixture : ScreenFixture
    {
        private SamlResponse _response;
        private DateTime _now;
        private SubjectConfirmation _confirmation;

        public SamlResponseFixture()
        {
            AddSelectionValues("SamlStatus", SamlStatus.Success.Uri.ToString(), SamlStatus.RequesterError.Uri.ToString(), SamlStatus.ResponderError.Uri.ToString());
        }

        protected override void beforeRunning()
        {
            var destination = Retrieve<SamlDestination>();


            _response = new SamlResponse
            {
                Id = Guid.NewGuid().ToString(),
                Status = SamlStatus.Success,
                Conditions = new ConditionGroup(),
                Authentication = new AuthenticationStatement(),
                Destination = (destination.DestinationUrl ?? Application.RootUrl).ToUri()
            };

            _now = Retrieve<ISystemTime>().UtcNow();
            _response.IssueInstant = new DateTimeOffset(_now);
        }

        public override void TearDown()
        {
            SamlEndpoint.SamlResponse = _response;

            Navigation.NavigateTo<SamlEndpoint>(x => x.get_saml_redirect());
            //IWebElement element = Driver.FindElement(By.Name("SamlResponse"));

            //var writer = new SamlResponseXmlWriter(_response);

            //element.SendKeys(writer.Write().OuterXml);

            //Driver.FindElement(By.Id("saml-submit")).Click();
        }

        [FormatAs("The issuer is {issuer}")]
        public void IssuerIs(Uri issuer)
        {
            _response.Issuer = issuer;
        }

        [FormatAs("The saml status is {status}")]
        public void StatusIs([SelectionValues("SamlStatus")]string status)
        {
            _response.Status = SamlStatus.Get(status);
        }

        [FormatAs("The name is {type}/{format}/{name}")]
        public void TheNameIs([DefaultValue("NameID")]string type, [DefaultValue("urn:oasis:names:tc:SAML:2.0:nameid-format:persistent")]string format, string name)
        {
            _response.Subject.Name = new SamlName
            {
                Format = NameFormat.Get(format),
                Type = type.ToEnumValue<SamlNameType>(),
                Value = name
            };
        }

        [FormatAs("Add a subject confirmation with method {method} and recipient {recipient} that expires in {minutes} minutes")]
        public void AddConfirmation([DefaultValue("urn:oasis:names:tc:SAML:2.0:cm:bearer")]string method, Uri recipient, int minutes)
        {
            _confirmation = new SubjectConfirmation
            {
                Method = new Uri(method),
            };

            var data = new SubjectConfirmationData
            {
                NotOnOrAfter = new DateTimeOffset(_now.AddMinutes(minutes)),
                Recipient = recipient
            };

            _confirmation.Add(data);

            _response.Subject.Add(_confirmation);
        }

        [FormatAs("The Saml Response is valid from {minutesAgo} minutes ago until {minutesUntil} minutes from now")]
        public void ResponseIsValid(int minutesAgo, int minutesUntil)
        {
            _response.Conditions.NotBefore = new DateTimeOffset(_now.AddMinutes(-minutesAgo));
            _response.Conditions.NotOnOrAfter = new DateTimeOffset(_now.AddMinutes(minutesUntil));
        }

        [FormatAs("Audience is restricted to {uri}")]
        public void RestrictAudience(Uri uri)
        {
            _response.Conditions.RestrictToAudience(uri);            
        }

        [ExposeAsTable("The attributes are")]
        public void Attributes(string Key, string Value)
        {
            _response.AddAttribute(Key, Value);
        }

        public SamlResponse Response
        {
            get { return _response; }
        }
    }
}