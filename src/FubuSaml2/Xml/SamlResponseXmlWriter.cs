using System;
using System.Collections.Generic;
using System.Xml;
using FubuCore.Configuration;
using System.Linq;

namespace FubuSaml2.Xml
{
    public class SamlResponseXmlWriter : ReadsSamlXml
    {
        private readonly SamlResponse _response;
        private readonly XmlDocument _document;
        private readonly XmlElement _root;
        private XmlElementStack _assertion;

        public SamlResponseXmlWriter(SamlResponse response)
        {
            _response = response;

            var nameTable = new NameTable();
            var namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("saml", AssertionXsd);
            namespaceManager.AddNamespace("samlp", ProtocolXsd);

            _document = new XmlDocument(nameTable);
            _root = createElement("Response", ProtocolXsd);
            _document.AppendChild(_root);

            _root.SetAttribute("Version", "2.0");
        }

        private XmlElement createElement(string name, string xsd = AssertionXsd)
        {
            return _document.CreateElement(name, xsd);
        }

        private XmlElementStack start(string name, string xsd = AssertionXsd)
        {
            var stack = new XmlElementStack(this, xsd);
            stack.Push(name);

            return stack;
        }

        public XmlDocument Write()
        {
            writeRootAttributes();
            writeStatusCode();
            writeIssuer();

            writeAssertion();
            writeSubject();
            writeConditions();

            return _document;
        }

        private void writeConditions()
        {
            _assertion.Push(Conditions)
                      .Attr(NotBeforeAtt, _response.Conditions.NotBefore)
                      .Attr(NotOnOrAfterAtt, _response.Conditions.NotOnOrAfter);

            _response.Conditions.Conditions.OfType<AudienceRestriction>().Each(x => {
                _assertion.Push(AudienceRestriction);
                x.Audiences.Each(a => _assertion.Add(Audience).Text(a.ToString()));
                _assertion.Pop();
            });
        }

        private void writeAssertion()
        {
            _assertion = start("Assertion")
                .Attr(ID, _response.Id)
                .Attr(IssueInstant, _response.IssueInstant);

            _assertion.Push(Issuer).InnerText = _response.Issuer.ToString();
            _assertion.Pop();
        }

        private void writeSubject()
        {
            _assertion.Push(Subject);
            var subjectName = _response.Subject.Name;

            // TODO -- going to need more 
            //                              .Attr("Format", "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent")
            //                              .Attr("NameQualifier", _response.Issuer);
            _assertion.Add(subjectName.Type.ToString())
                      .Text(subjectName.Value);

            _response.Subject.Confirmations.Each(confirmation => {
                _assertion.Push(SubjectConfirmation).Attr(MethodAtt, confirmation.Method);
                confirmation.ConfirmationData.Each(data => {
                    _assertion.Add(SubjectConfirmationData)
                              .Attr(NotOnOrAfterAtt, data.NotOnOrAfter)
                              .Attr(RecipientAtt, data.Recipient);
                });

                _assertion.Pop();
            });
        }

        private void writeIssuer()
        {
            start(Issuer).Text(_response.Issuer.ToString());
        }

        private void writeStatusCode()
        {
            start("Status", ProtocolXsd)
                .Push("StatusCode")
                .Attr("Value", _response.Status.Uri)
                .Attr("Version", "2.0");
        }

        private void writeRootAttributes()
        {
            _root.SetAttribute(ID, _response.Id);
            _root.SetAttribute(Destination, _response.Destination.ToString());
            _root.SetAttribute(IssueInstant, XmlConvert.ToString(_response.IssueInstant));
        }

        public class XmlElementStack : ReadsSamlXml
        {
            private readonly SamlResponseXmlWriter _parent;
            private readonly string _xsd;
            private readonly Stack<XmlElement> _elements = new Stack<XmlElement>(); 

            public XmlElementStack(SamlResponseXmlWriter parent, string xsd)
            {
                _parent = parent;
                _xsd = xsd;
                
                _elements.Push(_parent._document.DocumentElement);
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

            public void Pop()
            {
                _elements.Pop();
            }

            public XmlElement Add(string name, string xsd = null)
            {
                var element = _parent.createElement(name, xsd ?? _xsd);
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
                Current.Attr(name, value);

                return this;
            }

            public XmlElementStack Attr(string name, DateTimeOffset value)
            {
                Current.Attr(name, XmlConvert.ToString(value));

                return this;
            }

            public XmlElementStack Attr(string name, Uri value)
            {
                Current.Attr(name, value.ToString());

                return this;
            }
        }
    }
}