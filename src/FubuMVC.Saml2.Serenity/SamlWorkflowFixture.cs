using FubuMVC.Authentication;
using Serenity.Fixtures;
using StoryTeller;
using StoryTeller.Assertions;
using StoryTeller.Engine;

namespace FubuMVC.Saml2.Serenity
{
    public class SamlWorkflowFixture : ScreenFixture
    {
        [FormatAs("For relative url {relativeUrl}")]
        public void ForDestination(string relativeUrl)
        {
            var destination = new SamlDestination
            {
                DestinationUrl = relativeUrl
            };

            Store(destination);
        }

        [FormatAs("The browser should be on the home page")]
        public bool IsOnTheHomePage()
        {
            StoryTellerAssert.Fail(() => Driver.Url.TrimEnd('/') != Application.RootUrl.TrimEnd('/'), "Expected {0}, got {1}", Application.RootUrl.TrimEnd(), Driver.Url.TrimEnd());

            return true;
        }

        [FormatAs("The browser should be on the login page")]
        public bool IsOnTheLoginPage()
        {
            var expectedUrl = Application.Urls.UrlFor<LoginRequest>();
            StoryTellerAssert.Fail(!Driver.Url.StartsWith(expectedUrl), "The actual url is " + Driver.Url);

            return true;
        }

        [FormatAs("Go to the home page again")]
        public void GoHome()
        {
            Navigation.NavigateToHome();
        }

        [FormatAs("The browser is on relative url {url}")]
        public bool IsOnPage(string url)
        {
            StoryTellerAssert.Fail(!Driver.Url.EndsWith(url), "The actual url is " + Driver.Url);

            return true;
        }

        public IGrammar ReceiveSamlResponse()
        {
            return Embed<SamlResponseFixture>("Receive a SamlResponse");
        }
    }
}