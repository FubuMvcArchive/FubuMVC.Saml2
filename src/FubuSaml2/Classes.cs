using System;
using FubuCore.Binding.Values;

namespace FubuSaml2
{
    public interface ICondition
    {
        // TODO -- one time use
        // TODO -- ProxyRestriction
    }

    public class AuthenticationStatement
    {
        public DateTimeOffset Instant { get; set; }
        public string SessionIndex { get; set; }
        public DateTimeOffset SessionNotOnOrAfter { get; set; }

        public AuthenticationContext Context { get; set; }
    }

    public class AuthenticationContext
    {
        // AuthnContextDeclRef -- Uri
        // AuthnContextDecl authentication context declaration
    }

    public class AttributeStatement
    {
        
    }
}