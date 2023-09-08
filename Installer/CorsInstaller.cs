using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_Api.Installer
{
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection service, IConfiguration configuration)
        {
            service.AddCors(options =>
            {
                options.AddPolicy("AllowSpeificOrigins", builder =>
                {
                    builder.WithOrigins("https://www.w3schools.com", "http://localhost:3000/")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });

                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }
    }
}

/*
--- CORS ----

allow to access API for fetch data to often Domain

- สามารถ option allow ต่าวๆได้

*/