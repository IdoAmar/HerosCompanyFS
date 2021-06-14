using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Services.Encrypters
{
    public interface IPasswordHasherService
    {
        (byte[] hashedPassword, byte[] salt) HashPassword(string password);

        bool VerifyPassword(string passwordToVerify, byte[] salt, byte[] hashedPassword);
    }
}
