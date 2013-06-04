using System;
using System.Xml;
using FubuCore.Binding;
using FubuMVC.Core.Http;
using FubuMVC.Core.Security;
using FubuMVC.Core.Urls;
using FubuSaml2;
using FubuSaml2.Encryption;
using FubuSaml2.Xml;
using HtmlTags;

namespace FubuMVC.Saml2.Serenity
{
    [NotAuthenticated]
    public class SamlEndpoint
    {
        private readonly IRequestData _requestData;
        private readonly SamlResponseRedirector _redirector;
        private readonly IUrlRegistry _urls;
        private readonly ISamlResponseWriter _writer;

        public SamlEndpoint(IRequestData requestData, SamlResponseRedirector redirector, IUrlRegistry urls, ISamlResponseWriter writer)
        {
            _requestData = requestData;
            _redirector = redirector;
            _urls = urls;
            _writer = writer;
        }

        public static SamlResponse SamlResponse { get; set; }

        public HtmlDocument post_test_saml()
        {
            var document = new XmlDocument();

            var xml = _requestData.Value("SamlResponse") as string;
            
            document.LoadXml(xml);

            var response = new SamlResponseXmlReader(document).Read();
            return _redirector.WriteRedirectionHtml(response);
        }

        public HtmlDocument get_saml_redirect()
        {
            if (SamlResponse == null)
            {
                throw new InvalidOperationException("No SamlResponse is known!");
            }

            return _redirector.WriteRedirectionHtml(SamlResponse);
        }


        public HtmlDocument get_saml_poster()
        {
            var document = new HtmlDocument();
            document.Title = "Saml Poster";

            var form = new FormTag(_urls.UrlFor<SamlEndpoint>(x => x.post_test_saml()));

            document.Push(form);
            var textarea = form.Add("textarea").Attr("name", "SamlResponse").Attr("rows", 20).Attr("cols", "100");
            if (SamlResponse != null)
            {
                var xml = _writer.Write(SamlResponse);
                textarea.Attr("value", xml);

                SamlResponse = null;
            }

            form.Add("input").Attr("type", "submit").Attr("value", "Submit").Id("saml-submit");

            return document;
        }
    }
}