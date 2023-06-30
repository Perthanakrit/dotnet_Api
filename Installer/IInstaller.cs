using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_Api.Installer
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection service, IConfiguration configuration);
    }
}