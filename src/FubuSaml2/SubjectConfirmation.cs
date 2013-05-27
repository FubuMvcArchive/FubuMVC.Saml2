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
            Method = element.GetAttribute(MethodAtt).ToUri();
            ConfirmationData = buildData(element).ToArray();
        }

        private IEnumerable<SubjectConfirmationData> buildData(XmlElement element)
        {
            foreach (XmlElement dataElement in element.GetElementsByTagName(SubjectConfirmationData, AssertionXsd))
            {
                yield return new SubjectConfirmationData(dataElement);
            }
        } 

        public Uri Method { get; set; }

        public SamlName Name { get; set; }

        public SubjectConfirmationData[] ConfirmationData { get; set; }

        protected bool Equals(SubjectConfirmation other)
        {
            return Equals(Method, other.Method) && Equals(Name, other.Name) && ConfirmationData.SequenceEqual(other.ConfirmationData);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SubjectConfirmation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Method != null ? Method.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ConfirmationData != null ? ConfirmationData.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}