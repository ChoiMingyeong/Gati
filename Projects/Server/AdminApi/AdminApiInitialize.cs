using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerLib;

namespace AdminApi
{
    public class AdminApiInitialize
    {
        [InitializeConfigureServices]
        private static void InitializeOnStartup(IServiceCollection services)
        {
            if (ServerLib.App.Services.Contains("AdminApi") == false)
            {
                Logger.Default.LogInformation("fail load admin api");
            }
            else
            {
                Logger.Default.LogInformation("admin api initialize");
            }
        }
    }
}
