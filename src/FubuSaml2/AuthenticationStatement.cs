using System;
using System.Xml;
using FubuSaml2.Xml;

namespace FubuSaml2
{
    public class AuthenticationStatement : ReadsSamlXml
    {
        public AuthenticationStatement()
        {
        }

        public AuthenticationStatement(XmlDocument document)
        {
            var element = document.FindChild(AuthnStatement);
            if (element == null) return;

            Instant = element.ReadAttribute<DateTimeOffset>(AuthnInstant);
            SessionIndex = element.ReadAttribute<string>(SessionIndexAtt);
            SessionNotOnOrAfter = element.ReadAttribute<DateTimeOffset>(SessionNotOnOrAfterAtt);
        }

        public DateTimeOffset Instant { get; set; }
        public string SessionIndex { get; set; }
        public DateTimeOffset? SessionNotOnOrAfter { get; set; }


    }
}