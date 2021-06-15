using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HerosCompanyApi.Services.Encrypters
{
    public class RFCSHAPasswordHasherService : IPasswordHasherService
    {
        public (byte[] hashedPassword, byte[] salt) HashPassword(string password)
        {

            byte[] salt = new byte[512 / 8];
            byte[] hashedPassword;
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            using(var rfc = new Rfc2898DeriveBytes(password,salt,10000,HashAlgorithmName.SHA256))
            {
                hashedPassword = rfc.GetBytes(64);
            }
            return (hashedPassword, salt);
        }

        public bool VerifyPassword(string passwordToVerify, byte[] salt, byte[] hashedPassword)
        {
            using(var rfc = new Rfc2898DeriveBytes(passwordToVerify, salt, 10000, HashAlgorithmName.SHA256))
            {
                var hashedPasswordToVerify = rfc.GetBytes(64);
                if (hashedPasswordToVerify.SequenceEqual(hashedPassword))
                    return true;
                return false;
            }
        }
    }
}
