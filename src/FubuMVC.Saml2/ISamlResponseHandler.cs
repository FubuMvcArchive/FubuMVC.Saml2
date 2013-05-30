using FubuSaml2;

namespace FubuMVC.Saml2
{
    public interface ISamlResponseHandler
    {
        bool CanHandle(SamlResponse response);
        void Handle(ISamlDirector director, SamlResponse response);
    }
}