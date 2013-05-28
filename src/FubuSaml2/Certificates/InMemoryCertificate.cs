using System;

namespace FubuSaml2.Certificates
{
    public class InMemoryCertificate : ICertificate
    {
        public InMemoryCertificate()
        {
            Issuer = Guid.NewGuid().ToString();
            SerialNumber = Guid.NewGuid().ToString();
        }

        public string Issuer { get; set; }
        public string SerialNumber { get; set; }

        public bool IsVerified { get; set; }

    }
}