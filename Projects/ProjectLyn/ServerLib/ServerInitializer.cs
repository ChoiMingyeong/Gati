using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public static class ServerInitializer
    {
        private static bool _alreadyIniBeforeCreateHost = false;
        private static bool _alreadyInitAfterStartup = false;
        public static void InitBeforeCreateHost()
        {
            if (_alreadyIniBeforeCreateHost)
                return;
            _alreadyIniBeforeCreateHost = true;

            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            Logger.Init(App.EnvironmentName);
        }
        public static void InitAfterStartup(IServiceCollection services, IConfiguration configuration)
        {
            if (_alreadyInitAfterStartup)
                return;
            _alreadyInitAfterStartup = true;

            new ServerUtil(new Token());
        }
    }
}
