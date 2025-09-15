using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApi
{
    public partial class AdminResponse
    {
        public class Result
        {
            public int Code { get; set; }
            public string? Message { get; set; }
        }

        public class Base
        {
            public Result? Result { get; set; }
        }

        public class GetData : Base
        {
            public List<string>? Data { get; set; }
        }
    }
}
