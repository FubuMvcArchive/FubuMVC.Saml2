namespace FubuSaml2.Certificates
{
    public enum CertificateResult
    {
        CannotFindHandler,
        CertificateIsNotValid,
        CertificateDoesNotMatchIssuer,
        Validated
    }
}