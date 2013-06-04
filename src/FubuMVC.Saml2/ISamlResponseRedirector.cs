using FubuSaml2;
using HtmlTags;

namespace FubuMVC.Saml2
{
    public interface ISamlResponseRedirector
    {
        HtmlDocument WriteRedirectionHtml(SamlResponse response);
    }
}