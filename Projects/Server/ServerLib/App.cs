using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public static class App
    {
        private static string _environmentName;
        public static string ConfigEnvironment => EnvironmentName.ToLower();
        private static string _services;

        public static bool ContainAdminService => Services.Contains("AdminApi");

        public static string EnvironmentName
        {
            get
            {
                if (string.IsNullOrEmpty(_environmentName))
                {
                    _environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                }

                return _environmentName;
            }
        }
        public static string Services
        {
            get
            {
                if (string.IsNullOrEmpty(_services))
                    _services = Environment.GetEnvironmentVariable("SERVICE");

                return _services;
            }
        }
    }
}
