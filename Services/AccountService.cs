using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using dotnet_Api.Data;
using dotnet_Api.Entities;
using dotnet_Api.Installer;
using dotnet_Api.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly JWTSettings _jWTSettings;

        public AccountService(DatabaseContext databaseContext, JWTSettings jWTSettings)
        {
            _databaseContext = databaseContext;
            _jWTSettings = jWTSettings;
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
        public async Task<Account> Login(string username, string password)
        {
            var account = await _databaseContext.Accounts.Include(a => a.Role).SingleOrDefaultAsync(a => a.Username == username);

            if (account != null && VerifyPassword(account.Password, password))
            {
                return account;
            }
            return null;
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

        /// <summary>
        /// Verify password 
        /// <param name="hash">
        /// <param name="password">
        ///</summary>
        private bool VerifyPassword(string hash, string password)
        {
            var part = hash.Split(".", 2);
            if (part.Length != 2) return false;

            var salt = Convert.FromBase64String(part[0]); //database
            var hashedPassword = part[1]; // hashed password in database

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 258 / 8
            ));

            return hashedPassword == hashed;
        }

        public string GenerateToken(Account account)
        {
            // key is case-sensitive
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, account.Username),
                new Claim("role", account.Role.Name),
                new Claim("addtional", "todo"),

            };

            return BuildToken(claims); // นำเก็บไว้ที่ storage (web)
        }

        private string BuildToken(Claim[] claims)
        {
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jWTSettings.Expire));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jWTSettings.Issuer,
                audience: _jWTSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Account GetInfo(string accessToken)
        {
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            var username = token.Claims.First(claim => claim.Type == "sub").Value;
            var role = token.Claims.First(claim => claim.Type == "role").Value;

            var account = new Account
            {
                Username = username,
                Role = new Role
                {
                    Name = role
                }
            };

            return account;
        }

    }
}