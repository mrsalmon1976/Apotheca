using System;
using System.Collections.Generic;
using System.Text;

namespace Apotheca.Auth
{
    public static class AmazonCognitoHashCalculator
    {
        public static string GetSecretHash(string username, string appClientId, string appSecretKey)
        {
            var dataString = username + appClientId;

            var data = Encoding.UTF8.GetBytes(dataString);
            var key = Encoding.UTF8.GetBytes(appSecretKey);

            return Convert.ToBase64String(HmacSHA256(data, key));
        }

        public static byte[] HmacSHA256(byte[] data, byte[] key)
        {
            using (var shaAlgorithm = new System.Security.Cryptography.HMACSHA256(key))
            {
                var result = shaAlgorithm.ComputeHash(data);
                return result;
            }
        }
    }
}
