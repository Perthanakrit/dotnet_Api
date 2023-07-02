using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_Api.Installer
{
    public class JWTInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection service, IConfiguration configuration)
        {

            var jWTSettings = new JWTSettings();
            configuration.Bind(nameof(jWTSettings), jWTSettings);
            service.AddSingleton(jWTSettings); // jwtSettings มีหนึ่งเดียวเืท่านั้น

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jWTSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jWTSettings.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jWTSettings.Key)),
                        ClockSkew = TimeSpan.Zero

                    };
                });
        }

    }

    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Expire { get; set; }
    }

}