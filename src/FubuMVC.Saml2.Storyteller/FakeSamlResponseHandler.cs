using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Logging;
using FubuSaml2;

namespace FubuMVC.Saml2.Storyteller
{
    public class FakeSamlResponseHandler : BasicSamlResponseHandler
    {
        private static readonly IList<Uri> _audiences = new List<Uri>();

        public FakeSamlResponseHandler(ILogger logger) : base(logger)
        {
        }

        public override bool CanHandle(SamlResponse response)
        {
            return response.AudienceRestrictions.SelectMany(x => x.Audiences).Any(x => _audiences.Contains(x));
        }

        protected override string createLocalUser(SamlResponse response)
        {
            return response.Subject.Name.Value;
        }

        public static void Add(Uri audience)
        {
            _audiences.Add(audience);
        }

        public static void ClearAudiences()
        {
            _audiences.Clear();
        }
    }
}