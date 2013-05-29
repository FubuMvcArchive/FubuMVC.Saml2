using System;
using System.Collections.Generic;
using System.Linq;

namespace FubuSaml2.Validation
{
    /// <summary>
    /// The Saml2 spec considers the audience to be "Any", so we do too
    /// </summary>
    public class AudienceValidationRule : ISamlValidationRule
    {
        private readonly Uri[] _audiences;

        public AudienceValidationRule(Uri audience)
        {
            _audiences = new Uri[]{audience};
        }

        public AudienceValidationRule(params Uri[] audiences)
        {
            _audiences = audiences;
        }

        public IEnumerable<Uri> Audiences
        {
            get { return _audiences; }
        }

        public void Validate(SamlResponse response)
        {
            var restrictions = response.AudienceRestrictions;
            if (!restrictions.Any()) return;

            if (!_audiences.Any(x => restrictions.Any(r => r.Audiences.Contains(x))))
            {
                response.LogError(new SamlError(SamlValidationKeys.AudiencesDoNotMatch));
            }
        }
    }
}