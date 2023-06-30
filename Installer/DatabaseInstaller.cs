using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_Api.Installer
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection service, IConfiguration configuration)
        {
            //DI dependeny Injection
            service.AddDbContext<DatabaseContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("ConnectionSQLServer")));
        }
    }
}