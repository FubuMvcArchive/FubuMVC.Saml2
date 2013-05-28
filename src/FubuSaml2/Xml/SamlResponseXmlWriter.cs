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
        private XmlDocument _document;
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
            _root = _document.CreateElement("Response", ProtocolXsd);
            _document.AppendChild(_root);

            _root.SetAttribute("Version", "2.0");
        }

        private XmlElementStack start(string name, string xsd = AssertionXsd)
        {
            var stack = new XmlElementStack(_document, xsd);
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
            writeAuthenticationStatement();
            writeAttributes();

            return _document;
        }

        private void writeAttributes()
        {
            var root = start(AttributeStatement);
            var keys = _response.Attributes.GetKeys();

            keys.Each(key => {
                var attributeElement = root.Child(Attribute)
                    .Attr(Name, key)
                    .Attr(NameFormatAtt, "urn:oasis:names:tc:SAML:2.0:attrname-format:basic".ToUri());
                
                
                var value = _response.Attributes.Get(key);

                // TODO -- need to get a UT on this.
                var enumerable = value as IEnumerable<string>;
                if (enumerable != null)
                {
                    enumerable.Each(x => {
                        attributeElement.Add(AttributeValue).InnerText = x;
                    });
                }
                else
                {
                    attributeElement.Add(AttributeValue).InnerText = value as string;
                }
            });
        }

        private void writeAuthenticationStatement()
        {
            var authXmlStack = _assertion.Child(AuthnStatement);
            var statement = _response.Authentication;
            authXmlStack.Attr(AuthnInstant, statement.Instant);
            authXmlStack.Attr(SessionIndexAtt, statement.SessionIndex);
            if (statement.SessionNotOnOrAfter != null)
            {
                authXmlStack.Attr(SessionNotOnOrAfterAtt, statement.SessionNotOnOrAfter.Value);
            }

            authXmlStack.Push(AuthnContext);
            if (statement.DeclarationReference != null)
            {
                authXmlStack.Add(AuthnContextDeclRef).Text(statement.DeclarationReference.ToString());
            }
        }

        private void writeConditions()
        {
            var conditions = _assertion.Child(ConditionsElem)
                      .Attr(NotBeforeAtt, _response.Conditions.NotBefore)
                      .Attr(NotOnOrAfterAtt, _response.Conditions.NotOnOrAfter);

            _response.Conditions.Conditions.OfType<AudienceRestriction>().Each(x => {
                var restriction = conditions.Child(AudienceRestriction);
                x.Audiences.Each(a => restriction.Add(Audience).Text(a.ToString()));
            });
        }

        private void writeAssertion()
        {
            _assertion = start(AssertionElem)
                .Attr(ID, _response.Id)
                .Attr(IssueInstant, _response.IssueInstant);

            _assertion.Push(Issuer).InnerText = _response.Issuer.ToString();
            _assertion.Pop();
        }

        private void writeSubject()
        {
            var subject = _assertion.Child(Subject);
            var subjectName = _response.Subject.Name;

            // TODO -- going to need more 
            //                              .Attr("Format", "urn:oasis:names:tc:SAML:2.0:nameid-format:persistent")
            //                              .Attr("NameQualifier", _response.Issuer);
            subject.Add(subjectName.Type.ToString())
                      .Text(subjectName.Value)
                      .Attr(FormatAtt, subjectName.Format.Uri);

            _response.Subject.Confirmations.Each(confirmation => {
                subject.Push(SubjectConfirmation).Attr(MethodAtt, confirmation.Method);
                confirmation.ConfirmationData.Each(data => {
                   subject.Add(SubjectConfirmationData)
                              .Attr(NotOnOrAfterAtt, data.NotOnOrAfter)
                              .Attr(RecipientAtt, data.Recipient);
                });

                subject.Pop();
            });
        }

        private void writeIssuer()
        {
            start(Issuer).Text(_response.Issuer.ToString());
        }

        private void writeStatusCode()
        {
            if (_response.Status == null)
            {
                throw new InvalidOperationException("Status is missing");
            }

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
    }
}