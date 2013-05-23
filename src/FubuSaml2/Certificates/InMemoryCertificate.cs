namespace FubuSaml2.Certificates
{
    public class InMemoryCertificate : ICertificate
    {
        public string Issuer { get; set; }
        public string SerialNumber { get; set; }

        public bool IsVerified { get; set; }

    }
}