﻿using Org.BouncyCastle.Crypto.Generators;

namespace SjxLogistics.Controllers.AuthenticationComponent
{
    public class BcryptPassHasher : IpasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
