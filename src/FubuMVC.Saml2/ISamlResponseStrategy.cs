using FubuSaml2;

namespace FubuMVC.Saml2
{
    public interface ISamlResponseStrategy
    {
        bool CanHandle(SamlResponse response);
        void Handle(ISamlDirector director, SamlResponse response);
    }
}