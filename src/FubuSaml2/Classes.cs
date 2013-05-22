using System;
using System.Security.Cryptography.X509Certificates;

namespace FubuSaml2
{
    public enum SamlResponseStatus
    {
        Success,
        Failure
    }

    public class SamlResponse
    {
        public SamlResponseStatus Status { get; set; }
        public Uri Issuer { get; set; }
        public X509Certificate2 Signature { get; set; }

        public Subject Subject { get; set; }

        // valid if no conditions
        public ConditionGroup Conditions { get; set; }
    }


    // TODO -- one time use
    // TODO -- ProxyRestriction
    public interface ICondition
    {
        
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