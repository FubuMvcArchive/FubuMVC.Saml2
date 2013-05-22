using System;

namespace FubuSaml2
{
    public class AudienceRestriction : ICondition
    {
        public Uri[] Audiences { get; set; }
    }
}