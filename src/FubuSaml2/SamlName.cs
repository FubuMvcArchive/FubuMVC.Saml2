using System.Xml;
using FubuSaml2.Xml;

namespace FubuSaml2
{
    public class SamlName : ReadsSamlXml
    {
        public SamlName()
        {
        }

        public SamlName(XmlElement element)
        {
            // TODO -- read BaseID
            // TODO -- read EncryptedID


            // TODO -- add NameQualifier as URI
            // TODO -- add SPNameQualifier as URI
            // TODO -- add Format - urn:oasis:names:tc:SAML:2.0:nameid-format:persistent  <-- this matters!

            var name = element.FindChild(NameID, AssertionXsd);
            if (name != null)
            {
                Type = SamlNameType.NameID;
                Value = name.InnerText;
            }
        }

        public SamlNameType Type { get; set; }
        public string Value { get; set; }

        protected bool Equals(SamlName other)
        {
            return Type == other.Type && string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SamlName) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Type*397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, Value: {1}", Type, Value);
        }
    }
}