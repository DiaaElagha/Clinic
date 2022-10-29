using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Helper
{
    public static class EncryptionHelper
    {
        public static string CreatePassword(this string text) =>
            Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: text,
                    salt: RandomNumberGenerator.GetBytes(128 / 8),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));


    }
}
