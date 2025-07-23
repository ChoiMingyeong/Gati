using System.Diagnostics.CodeAnalysis;
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

        public List<DataTable> GetDataTables()
        {
            return [.. DataTables.Values];
        }

        public bool TryGetDataTable(Tsid tsid, [NotNullWhen(true)] out DataTable? dataTable)
        {
            if (DataTables.TryGetValue(tsid, out dataTable))
            {
                return true;
            }

            return false;
        }
    }
}
