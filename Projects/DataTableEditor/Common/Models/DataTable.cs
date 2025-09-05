using TSID.Creator.NET;

namespace Common.Models
{
    public class DataTable
    {
        public Tsid TSID { get; set; }

        public Version Version { get; set; } = new Version(1, 0, 0, 0);

        public string Name { get; set; } = string.Empty;

        public DataTable()
        {
            TSID = TsidCreator.GetTsid();
            Version = new Version(1, 0, 0, 0);
        }
    }
}
