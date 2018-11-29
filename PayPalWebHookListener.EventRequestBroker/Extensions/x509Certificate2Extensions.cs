using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RevolutionGolf.EventRequestBroker.Extensions
{
    //TODO: Clean up (i.e. get rid of) if not going to be used.
    public static class X509Certificate2Extensions
    {
        public static RSACryptoServiceProvider GetPublicKeyRsaCryptoServiceProvider(this X509Certificate2 certificate)
        {
            var rsa = certificate.GetRSAPublicKey();
            var rsaParameters = rsa.ExportParameters(false);

            var csp = new RSACryptoServiceProvider(rsa.KeySize, new CspParameters()
            {
                ProviderType = 24,
                ProviderName = "Microsoft Enhanced RSA and AES Cryptographic Provider",
            });
            csp.ImportParameters(rsaParameters);

            return csp;
        }

        public static RSACryptoServiceProvider GetPrivateKeyRsaCryptoServiceProvider(this X509Certificate2 certificate)
        {
            var rsa = certificate.GetRSAPrivateKey();
            var rsaParameters = rsa.ExportParameters(true);

            var csp = new RSACryptoServiceProvider(rsa.KeySize, new CspParameters()
            {
                ProviderType = 24,
                ProviderName = "Microsoft Enhanced RSA and AES Cryptographic Provider",
            });
            csp.ImportParameters(rsaParameters);


            return csp;
        }
    }
}
