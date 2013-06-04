using System.Collections.Generic;
using FubuMVC.Authentication;
using FubuSaml2;
using HtmlTags;

namespace FubuMVC.Saml2.Storyteller
{
    public class HomeEndpoint
    {
        public HtmlDocument Index()
        {
            var document = new HtmlDocument();
            document.Title = "The home page";

            document.Add("h1").Text("This is the home page");

            return document;
        }

        public HtmlDocument get_login(LoginRequest request)
        {
            var document = new HtmlDocument();
            document.Title = "Login Page";

            document.Add("h1").Text("This is the login page");

            return document;
        }

        public HtmlDocument get_failure_page(SamlResponse response)
        {
            var document = new HtmlDocument();
            document.Title = "Failure Page";

            document.Add("h1").Text("This is the failure page");

            document.Push("ul");

            response.Errors.Each(x => document.Add("li").Text(x.Message));

            return document;
        }
    }
}