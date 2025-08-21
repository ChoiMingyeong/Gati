using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApi
{
    public partial class AdminRequest
    {
        public class SendData
        {
            public int Data { get; set; }
        }

        public class LoginData
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }
    }
}
