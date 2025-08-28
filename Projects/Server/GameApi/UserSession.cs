using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameApi
{
    public class UserSession
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }  // 계정 ID
        public string Name { get; set; }    // 닉네임
    }
}
