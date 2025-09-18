using System;

namespace ServerLib.Model
{
    public class account
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime LoginTime { get; set; }
        public int Exp { get; set; }
    }
}