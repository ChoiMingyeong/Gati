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
        public static void InitBeforeCreateHost()
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            Logger.Init(App.EnvironmentName);
        }
    }
}
