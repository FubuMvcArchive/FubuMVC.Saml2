using System.Collections.Generic;
using System.Linq;
using FubuCore.Binding;
using FubuMVC.Authentication;
using FubuSaml2.Encryption;
using FubuSaml2.Validation;

namespace FubuMVC.Saml2
{
    public class SamlAuthenticationStrategy : IAuthenticationStrategy
    {
        public static readonly AuthResult DoesNotApply = new AuthResult{Success = false};

        private readonly IRequestData _requestData;
        private readonly ISamlDirector _director;
        private readonly ISamlResponseReader _reader;
        private readonly IEnumerable<ISamlValidationRule> _rules;
        private readonly IEnumerable<ISamlResponseHandler> _strategies;

        public SamlAuthenticationStrategy(IRequestData requestData, ISamlDirector director, ISamlResponseReader reader, IEnumerable<ISamlValidationRule> rules, IEnumerable<ISamlResponseHandler> strategies)
        {
            _requestData = requestData;
            _director = director;
            _reader = reader;
            _rules = rules;
            _strategies = strategies;
        }

        public AuthResult TryToApply()
        {
            AuthResult result = AuthResult.Failed();

            // TODO -- make sure we've got error handling here
            _requestData.Value("SamlResponse", v => {
                // TODO -- log here
                var xml = v.RawValue as string;
                ProcessSamlResponseXml(xml);

                result = _director.Result();
            });

            return result;
        }

        public virtual void ProcessSamlResponseXml(string xml)
        {
            var response = _reader.Read(xml);

            _rules.Each(x => x.Validate(response));

            // TODO -- Make sure there's a default one in here?
            var handler = _strategies.First(x => x.CanHandle(response));
            // TODO -- do something if we cannot select a handler.  Default message maybe
            handler.Handle(_director, response);
        }

        public bool Authenticate(LoginRequest request)
        {
            return false;
        }
    }
}