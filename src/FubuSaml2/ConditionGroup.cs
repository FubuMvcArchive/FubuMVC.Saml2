using System;
using System.Linq;
using System.Xml;
using FubuSaml2.Xml;

namespace FubuSaml2
{
    public class ConditionGroup : ReadsSamlXml
    {
        // Can be multiples -- if multiples, has to be AND/ALL

        public ConditionGroup()
        {
        }

        public ConditionGroup(XmlElement element)
        {
            NotBefore = element.ReadAttribute<DateTimeOffset>(NotBeforeAtt);
            NotOnOrAfter = element.ReadAttribute<DateTimeOffset>(NotOnOrAfterAtt);

            // TODO -- couple other kinds of conditions here
            Conditions = readAudiences(element);
        }

        private AudienceRestriction[] readAudiences(XmlElement conditions)
        {
            return conditions
                .Children(AudienceRestriction, AssertionXsd)
                .Select(elem =>
                {
                    var audiences = elem.Children(Audience, AssertionXsd).Select(x => x.InnerText.ToUri()).ToArray();
                    return new AudienceRestriction
                    {
                        Audiences = audiences
                    };
                }).ToArray();
        }

        public DateTimeOffset NotBefore { get; set; }
        public DateTimeOffset NotOnOrAfter { get; set; }

        public ICondition[] Conditions { get; set; }
    }
}