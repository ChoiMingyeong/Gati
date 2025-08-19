using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{ 
    public class ServerUtil
    {
        public static ServerUtil Instance { get; private set; }

        public Token Token { get; }

        public ServerUtil(Token token)
        {
            Instance = this;
            Token = token; 
        }
    }
}
