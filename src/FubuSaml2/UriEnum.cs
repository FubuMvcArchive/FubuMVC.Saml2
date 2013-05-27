using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuSaml2.Xml;

namespace FubuSaml2
{
    public interface UriValue
    {
        Uri Uri { get; }
    }

    public abstract class UriEnum<T> : UriValue where T : UriValue
    {
        private readonly Uri _uri;
        private readonly string _description;
        private static readonly IList<T> _all = new List<T>();

        protected UriEnum(string uri, string description = null)
        {
            _uri = uri.ToUri();
            _description = description;

            _all.Add(this.As<T>());
        }

        public Uri Uri
        {
            get { return _uri; }
        }

        public string Description
        {
            get { return _description; }
        }

        public static T Get(string uriString)
        {
            return _all.FirstOrDefault(x => x.Uri == uriString.ToUri());
        }
    }
}