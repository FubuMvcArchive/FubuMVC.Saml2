using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FubuSaml2.Xml;

namespace FubuSaml2
{
    public class SubjectConfirmation : ReadsSamlXml
    {
        public SubjectConfirmation()
        {
        }

        public SubjectConfirmation(XmlElement element)
        {
            Method = element.GetAttribute("Method").ToUri();
            ConfirmationData = buildData(element).ToArray();
        }

        private IEnumerable<SubjectConfirmationData> buildData(XmlElement element)
        {
            foreach (XmlElement dataElement in element.GetElementsByTagName("SubjectConfirmationData", AssertionXsd))
            {
                yield return new SubjectConfirmationData(dataElement);
            }
        } 

        public Uri Method { get; set; }

        public SamlName Name { get; set; }

        public SubjectConfirmationData[] ConfirmationData { get; set; }
    }
}