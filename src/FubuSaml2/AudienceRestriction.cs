using System;
using System.Linq;

namespace FubuSaml2
{
    public class AudienceRestriction : ICondition
    {
        public Uri[] Audiences { get; set; }

        public void Add(Uri audience)
        {
            if (Audiences == null)
            {
                Audiences = new Uri[] {audience};
            }
            else
            {
                Audiences = Audiences.Union(new Uri[] {audience}).ToArray();
            }
        }
    }
}