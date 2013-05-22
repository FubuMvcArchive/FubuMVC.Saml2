using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FubuSaml2
{
    public class Subject : ReadsSamlXml
    {
        public Subject()
        {
        }

        public Subject(XmlElement element)
        {
            Name = new SamlName(element);
            Confirmations = buildConfirmations(element).ToArray();
        }

        private IEnumerable<SubjectConfirmation> buildConfirmations(XmlElement element)
        {
            foreach (XmlElement confirmationElement in element.GetElementsByTagName("SubjectConfirmation", AssertionXsd))
            {
                yield return new SubjectConfirmation(confirmationElement);
            }
        } 

        public SamlName Name { get; set; }
    
        // can have multiple confirmations

        public SubjectConfirmation[] Confirmations { get; set; }
    }
}