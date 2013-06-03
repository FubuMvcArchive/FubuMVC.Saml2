using System;
using System.Collections.Generic;
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
            readAudiences(element).Each(Add);
        }

        private IEnumerable<AudienceRestriction> readAudiences(XmlElement conditions)
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
                });
        }

        public DateTimeOffset NotBefore { get; set; }
        public DateTimeOffset NotOnOrAfter { get; set; }

        public ICondition[] Conditions
        {
            get { return _conditions.ToArray(); }
            set
            {
                _conditions.Clear();
                _conditions.AddRange(value);
            }
        }
        private readonly IList<ICondition> _conditions = new List<ICondition>();

        public void Add(ICondition condition)
        {
            _conditions.Add(condition);
        }

        public void RestrictToAudience(Uri uri)
        {
            var restriction = new AudienceRestriction();
            restriction.Add(uri);

            Add(restriction);
        }
    }
}