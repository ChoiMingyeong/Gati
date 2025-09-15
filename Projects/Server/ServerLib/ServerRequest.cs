using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class ServerRequest
    {
        public class ServerInfo
        {
            public string? MachineName { get; set; }
            public string? ServiceApi { get; set; }
            public int UserCount { get; set; }
            public DateTime Now { get; set; }
        }
    }
}
