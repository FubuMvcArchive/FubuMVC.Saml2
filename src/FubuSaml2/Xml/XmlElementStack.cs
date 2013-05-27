using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using FubuCore;

namespace FubuSaml2.Xml
{
    public class XmlElementStack : ReadsSamlXml
    {
        private readonly string _xsd;
        private readonly Stack<XmlElement> _elements = new Stack<XmlElement>();
        private readonly XmlDocument _document;

        public XmlElementStack(XmlDocument document, string xsd)
        {
            _document = document;
            _xsd = xsd;
                
            _elements.Push(_document.DocumentElement);
        }

        public XmlElementStack(XmlElement starting, string xsd)
        {
            _document = starting.OwnerDocument;
            _elements.Push(starting);
            _xsd = xsd;
        }

        public XmlElement Current
        {
            get { return _elements.Peek(); }
        }

        public XmlElement Push(string name, string xsd = null)
        {
            var element = Add(name, xsd);

            _elements.Push(element);

            return element;
        }

        public XmlElementStack Child(string name, string xsd = null)
        {
            var element = Add(name, xsd);
            return new XmlElementStack(element, xsd ?? _xsd);
        }

        public void Pop()
        {
            _elements.Pop();
        }

        public XmlElement Add(string name, string xsd = null)
        {
            var element = _document.CreateElement(name, xsd ?? _xsd);
            Current.AppendChild(element);

            return element;
        }

        public XmlElementStack Text(string text)
        {
            Current.InnerText = text;
            return this;
        }

        public XmlElementStack Attr(string name, string value)
        {
            if (value.IsNotEmpty())
            {
                Current.Attr(name, value);
            }
            return this;
        }

        public XmlElementStack Attr(string name, DateTimeOffset value)
        {
            Current.Attr(name, XmlConvert.ToString(value));

            return this;
        }

        public XmlElementStack Attr(string name, Uri value)
        {
            if (value != null)
            {
                Current.Attr(name, value.ToString());
            }

            return this;
        }
    }
}