

using System;
using System.Collections.Generic;
using FubuCore.Dates;
using FubuLocalization;
using FubuSaml2.Certificates;

namespace FubuSaml2.Validation
{
    public class SamlValidationKeys : StringToken
    {
        public static readonly SamlValidationKeys TimeFrameDoesNotMatch = new SamlValidationKeys("Outside the valid time frame of this Saml Response");
        public static readonly SamlValidationKeys CannotMatchIssuer = new SamlValidationKeys("The certificate does not match the issuer");
        public static readonly SamlValidationKeys NoValidCertificates = new SamlValidationKeys("No valid certificates were found");
        public static readonly SamlValidationKeys ValidCertificate = new SamlValidationKeys("The certificate was valid");


        protected SamlValidationKeys(string defaultValue) : base(null, defaultValue, namespaceByType:true)
        {
        }
    }

    public interface ISamlValidationRule
    {
        void Validate(SamlResponse response);
    }

    public class ConditionTimeFrame : ISamlValidationRule
    {
        private readonly ISystemTime _systemTime;

        public ConditionTimeFrame(ISystemTime systemTime)
        {
            _systemTime = systemTime;
        }

        public void Validate(SamlResponse response)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AudienceRestriction : ISamlValidationRule
    {
        public AudienceRestriction(Uri audience)
        {
            
        }

        public AudienceRestriction(IEnumerable<Uri> audiences)
        {
        }

        public void Validate(SamlResponse response)
        {
            throw new System.NotImplementedException();
        }
    }

    public class CertificateValidation : ISamlValidationRule
    {
        private readonly ICertificateService _service;

        public CertificateValidation(ICertificateService service)
        {
            _service = service;
        }

        public void Validate(SamlResponse response)
        {
            throw new NotImplementedException();
        }
    }

    public class SignatureIsRequired : ISamlValidationRule
    {
        public void Validate(SamlResponse response)
        {
            throw new NotImplementedException();
        }
    }

}