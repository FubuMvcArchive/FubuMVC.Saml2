namespace FubuSaml2.Certificates
{
    public interface ICertificateValidator
    {
        CertificateResult Validate(SamlResponse response);
    }
}