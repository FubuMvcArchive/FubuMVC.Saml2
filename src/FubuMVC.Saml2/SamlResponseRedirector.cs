using FubuSaml2;
using FubuSaml2.Encryption;
using HtmlTags;

namespace FubuMVC.Saml2
{
    public class SamlResponseRedirector
    {
        private readonly ISamlResponseWriter _writer;

        public SamlResponseRedirector(ISamlResponseWriter writer)
        {
            _writer = writer;
        }

        public HtmlDocument WriteRedirectionHtml(SamlResponse response)
        {
            var responseString = _writer.Write(response);

            return new SamlResponseRedirectionDocument(responseString, response.Destination.ToString());
        } 
    }

    public class SamlResponseRedirectionDocument : HtmlDocument
    {
        public SamlResponseRedirectionDocument(string response, string destination)
        {
            Title = "Saml2 Response Redirection";

            var form = new FormTag(destination);

            Push(form);

            var hiddenTag = new HiddenTag().Attr("name", "SamlResponse")
                                           .Attr("value", response);

            Add(hiddenTag);

            Pop();
        }
    }
}