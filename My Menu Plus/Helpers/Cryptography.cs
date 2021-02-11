using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace My_Menu_Plus.Helpers
{
    internal sealed class Cryptography
    {
        /// <summary>
        /// Produces a hash from plain text
        /// </summary>
        /// <param name="plainText">string based plain text</param>
        /// <returns>Hashed string</returns>
        internal static string Hash(string plainText)
        {
            // Declare variables required
            const int SaltSize = 16;
            const int HashSize = 20;
            const int passwordIterations = 42532;

            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //create hash
            var pbkdf2 = new Rfc2898DeriveBytes(plainText, salt, passwordIterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            return base64Hash;
        }

        /// <summary>
        /// verify a password against a hash
        /// </summary>
        /// <param name="password">the password</param>
        /// <param name="hashedPassword">the hash</param>
        /// <returns>could be verified?</returns>
        internal static bool VerifyHash(string password, string hashedPassword)
        {
            // Declare variables required
            const int SaltSize = 16;
            const int HashSize = 20;
            const int iterations = 42532;

            // Get hashbytes
            var hashBytes = Convert.FromBase64String(hashedPassword);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}