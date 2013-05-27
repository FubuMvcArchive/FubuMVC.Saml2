namespace FubuSaml2.Xml
{
    public class ReadsSamlXml
    {
        public const string AssertionXsd = "urn:oasis:names:tc:SAML:2.0:assertion";
        public const string ProtocolXsd = "urn:oasis:names:tc:SAML:2.0:protocol";
        public const string EncryptedXsd = "http://www.w3.org/2001/04/xmlenc#";

        protected const string ID = "ID";
        protected const string Destination = "Destination";
        protected const string IssueInstant = "IssueInstant";
        protected const string StatusCode = "StatusCode";
        protected const string Value = "Value";
        protected const string AttributeStatement = "AttributeStatement";
        protected const string Name = "Name";
        protected const string AttributeValue = "AttributeValue";
        protected const string Attribute = "Attribute";
        protected const string Signature = "Signature";
        protected const string Issuer = "Issuer";
        protected const string ConditionsElem = "Conditions";
        protected const string Subject = "Subject";
        protected const string NameID = "NameID";
        protected const string SubjectConfirmation = "SubjectConfirmation";
        protected const string SubjectConfirmationData = "SubjectConfirmationData";
        protected const string MethodAtt = "Method";

        protected const string NotOnOrAfterAtt = "NotOnOrAfter";
        protected const string RecipientAtt = "Recipient";
        protected const string NotBeforeAtt = "NotBefore";
        protected const string AudienceRestriction = "AudienceRestriction";
        protected const string Audience = "Audience";
        protected const string FormatAtt = "Format";
    }
}