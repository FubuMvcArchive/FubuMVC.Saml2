using FubuSaml2;
using FubuSaml2.Encryption;
using HtmlTags;

namespace FubuMVC.Saml2
{
    public class SamlResponseRedirector : ISamlResponseRedirector
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
}