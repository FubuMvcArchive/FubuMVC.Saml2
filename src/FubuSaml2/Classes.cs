using System;
using System.Collections.Generic;
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

    public enum SamlNameType
    {
        NameID,
        BaseID,
        EncryptedID
    }

    public class SamlName
    {
        public SamlNameType Type { get; set; }
        public string Value { get; set; }
    }

    public class Subject
    {
        public SamlName Name { get; set; }
    
        // can have multiple confirmations

        public SubjectConfirmation[] Confirmations { get; set; }
    }

    public class SubjectConfirmation
    {
        public string Method { get; set; }

        public SamlName Name { get; set; }

        public SubjectConfirmationData[] ConfirmationData { get; set; }
    }

    public class SubjectConfirmationData
    {
        public DateTimeOffset? NotBefore { get; set; }
        public DateTimeOffset? NotOnOrBefore { get; set; }
        public Uri Recipient { get; set; }
        public string InResponseTo { get; set; }
        public string Address { get; set; }

        // Change this to IValueSource I think.  You could have custom attributes
        // or custom elements
        public IDictionary<string, string> Attributes { get; set; }
    }





    // Can be multiples -- if multiples, has to be AND/ALL
    public class ConditionGroup
    {
        public DateTimeOffset NotBefore { get; set; }
        public DateTimeOffset NotOnOrAfter { get; set; }

        public ICondition[] Conditions { get; set; }
    }

    // TODO -- one time use
    // TODO -- ProxyRestriction
    public interface ICondition
    {
        
    }

    public class AudienceRestriction : ICondition
    {
        public Uri[] Audiences { get; set; }
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