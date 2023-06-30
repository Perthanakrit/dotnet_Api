using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_Api.Installer
{
    // This is ex
    public static class InstallerExtension
    {
        public static void InstallServiceInAssembly(this IServiceCollection service, IConfiguration configuration)
        {
            var installer = typeof(Program).Assembly.ExportedTypes.Where(x =>
                                typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();//  ค้นหา assembly ใน project ที่มี Interface

            installer.ForEach(installer => installer.InstallServices(service, configuration));
        }

    }
}