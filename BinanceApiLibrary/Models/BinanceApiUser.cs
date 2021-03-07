using System;
using System.Security.Cryptography;
using System.Text;

namespace BinanceApiLibrary.Models
{
    public class BinanceApiUser
    {
        public string ApiPublicKey {get;}
        public string ApiPrivateKey { get; }

        private readonly HMAC _hmac;

        public BinanceApiUser(string publicApiKey, string privateApiKey)
        {
            ApiPublicKey = publicApiKey;
            ApiPrivateKey = privateApiKey;
            _hmac = new HMACSHA256(Encoding.UTF8.GetBytes(privateApiKey));
        }

        public string Sign(string totalParams)
        {
            if (_hmac == null)
            {
                throw new InvalidOperationException($"{nameof(BinanceApiUser)}.{nameof(Sign)} requires the user's API secret.");
            }
            byte[] hash;
            hash = _hmac.ComputeHash(Encoding.UTF8.GetBytes(totalParams));

            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }
    }
}
