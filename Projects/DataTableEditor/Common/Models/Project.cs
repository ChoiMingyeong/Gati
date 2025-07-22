using TSID.Creator.NET;

namespace Common.Models
{
    public class Project
    {
        public Tsid TSID { get; set; }

        public Dictionary<Tsid, DataTable> DataTables { get; set; } = [];

        public Project()
        {
            TSID = TsidCreator.GetTsid();
        }
    }
}
