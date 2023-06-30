using System.Security.Cryptography;
using dotnet_Api.Data;
using dotnet_Api.Entities;
using dotnet_Api.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace dotnet_Api.Services
{
    public class AccountSerivce : IAccountSerivce
    {
        private readonly DatabaseContext _databaseContext;

        public AccountSerivce(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        // เราจะ Hash password ด้วยการ solt hash ตอน register
        public async Task Register(Account account)
        {
            var existingAccount = await _databaseContext.Accounts.SingleOrDefaultAsync(a => a.Username == account.Username);

            if (existingAccount != null) throw new Exception("Existing account");

            account.Password = CreatePassWordHash(account.Password);
            _databaseContext.Accounts.Add(account);
            await _databaseContext.SaveChangesAsync();
        }
        public Task<Account> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        private string CreatePassWordHash(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 258 / 8
            ));

            return $"{Convert.ToBase64String(salt)}.{hashed}";

            /*
                hashed = solt + password
            */
        }

        private bool VerifyPassword(string hash, string password)
        {
            var part = hash.Split(".", 2);
            if (part.Length != 2) return false;

            var salt = Convert.FromBase64String(part[0]); //database
            var hashedPassword = part[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 258 / 8
            ));

            return hashedPassword == hashed;
        }
    }
}