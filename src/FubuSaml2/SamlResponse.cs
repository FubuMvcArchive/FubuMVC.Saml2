using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using FubuCore;
using FubuCore.Util;

namespace FubuSaml2
{
    public class SamlResponse
    {
        private readonly IDictionary<string, object> _attributes =
            new Dictionary<string, object>();

        public SamlResponse()
        {
            Attributes = new DictionaryKeyValues<object>(_attributes);
        }

        public SamlResponseStatus Status { get; set; }
        public Uri Issuer { get; set; }
        public IEnumerable<X509Certificate2> Certificate { get; set; }
        public SignatureStatus Signed { get; set; }

        public Subject Subject { get; set; }

        // valid if no conditions
        public ConditionGroup Conditions { get; set; }

        public IKeyValues<object> Attributes { get; private set; }
    
        public void AddAttribute(string key, string value)
        {
            if (_attributes.ContainsKey(key))
            {
                var existing = _attributes[key];
                if (existing is string)
                {
                    var list = new List<string> {existing as string, value};
                    _attributes[key] = list;
                }

                else
                {
                    existing.As<IList<string>>().Add(value);
                }
            }
            else
            {
                _attributes.Add(key, value);
            }
        }
    }

    public enum SignatureStatus
    {
        Signed,
        NotSigned,
        InvalidSignature
    }
}