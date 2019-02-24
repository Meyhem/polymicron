using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Pm.Core.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Pm.Core
{
    public class PasswordDerivationService : IPasswordDerivationService
    {
        private string GenSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private string DeriveKey(string password, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                             password: password,
                             salt: Encoding.UTF8.GetBytes(salt),
                             prf: KeyDerivationPrf.HMACSHA512,
                             iterationCount: 10000,
                             numBytesRequested: 256 / 8);

            return $"{salt}|{Convert.ToBase64String(valueBytes)}";
       
        }

        public string CreateHash(string password)
        {
            var salt = GenSalt();

            return DeriveKey(password, salt);
        }

        public bool VerifyHash(string hash, string password)
        {
            var components = hash.Split("|");
            if (components.Length != 2)
            {
                throw new ArgumentException("hash is not in PasswordDerivationService required format");
            }
            return hash == DeriveKey(password, components[0]);
        }
    }
}
