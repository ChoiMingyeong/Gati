using GatiDataTable.Core;
using TSID.Creator.NET;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dt = new DataTable("Test");
            
            var column = new DataTable.Column(TsidCreator.GetTsid(), typeof(string))
            {
                Name = "Name",
            };
            dt.Columns.Add(new DataTable.Column(TsidCreator.))
        }
    }
}
