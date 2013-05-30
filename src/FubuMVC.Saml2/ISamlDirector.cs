using FubuMVC.Authentication;
using FubuMVC.Core.Continuations;

namespace FubuMVC.Saml2
{
    public interface ISamlDirector
    {
        void SuccessfulUser(string username, FubuContinuation redirection = null);
        void FailedUser(FubuContinuation redirection = null);

        AuthResult Result();
    }
}