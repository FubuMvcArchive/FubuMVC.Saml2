using System;
using System.Collections.Generic;
using FubuCore.Binding;
using FubuMVC.Authentication;
using FubuMVC.Core.Continuations;
using FubuSaml2;
using FubuSaml2.Encryption;
using System.Linq;
using FubuSaml2.Validation;
using FubuCore;

namespace FubuMVC.Saml2
{
    public class SamlAuthenticationStrategy : IAuthenticationStrategy
    {
        public static readonly AuthResult DoesNotApply = new AuthResult{Success = false};

        private readonly IRequestData _requestData;
        private readonly ISamlDirector _director;
        private readonly ISamlResponseReader _reader;
        private readonly IEnumerable<ISamlValidationRule> _rules;
        private readonly IEnumerable<ISamlResponseStrategy> _strategies;

        public SamlAuthenticationStrategy(IRequestData requestData, ISamlDirector director, ISamlResponseReader reader, IEnumerable<ISamlValidationRule> rules, IEnumerable<ISamlResponseStrategy> strategies)
        {
            _requestData = requestData;
            _director = director;
            _reader = reader;
            _rules = rules;
            _strategies = strategies;
        }

        public AuthResult TryToApply()
        {
            // TODO -- make sure we've got error handling here
            _requestData.Value("SamlResponse", v => {
                // TODO -- log here
                var xml = v.RawValue as string;
                var response = _reader.Read(xml);

                _rules.Each(x => x.Validate(response));

                // Make sure there's a default one in here.
                var handler = _strategies.First(x => x.CanHandle(response));
                // TODO -- do something if we cannot select a handler.  Default message maybe
                handler.Handle(_director, response);
            });

            return _director.Result();
        }

        public bool Authenticate(LoginRequest request)
        {
            return false;
        }
    }

    // Make this go composition?
    public abstract class SamlResponseStrategyBase : ISamlResponseStrategy
    {
        private readonly IEnumerable<ISamlValidationRule> _rules;

        protected SamlResponseStrategyBase(IEnumerable<ISamlValidationRule> rules)
        {
            _rules = rules;
        }

        public abstract bool CanHandle(SamlResponse response);

        public void Handle(ISamlDirector director, SamlResponse response)
        {
            _rules.Each(x => x.Validate(response));

            if (response.Errors.Any())
            {
                director.FailedUser(redirection: redirectForFailure(response));
            }
            else
            {
                var userName = establishUser(response);
                director.SuccessfulUser(userName, continueForSuccess(response));
            }
        }

        protected virtual FubuContinuation redirectForFailure(SamlResponse response)
        {
            return null;
        }

        protected virtual string determineErrorMessage(SamlResponse response)
        {
            return response.Errors.Select(x => x.Message).Join("; ");
        }

        protected virtual FubuContinuation continueForSuccess(SamlResponse response)
        {
            return null;
        }

        protected abstract string establishUser(SamlResponse response);
    }
}