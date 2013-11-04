using System;
using System.Linq;

namespace FubuSaml2
{
    public class AudienceRestriction : ICondition
    {
        public string[] Audiences { get; set; }

        public void Add(string audience)
        {
            if (Audiences == null)
            {
                Audiences = new[] {audience};
            }
            else
            {
                Audiences = Audiences.Union(new[] {audience}).ToArray();
            }
        }
    }
}