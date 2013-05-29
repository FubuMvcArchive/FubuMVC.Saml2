namespace FubuSaml2.Validation
{
    public interface ISamlValidationRule
    {
        void Validate(SamlResponse response);
    }
}