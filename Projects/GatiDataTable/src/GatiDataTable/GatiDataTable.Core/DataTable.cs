
using TSID.Creator.NET;

namespace GatiDataTable.Core
{
    public sealed partial class DataTable : IDataTable
    {
        public ID ID { get; init; }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _name = value;
            }
        }
        private string _name = string.Empty;

        public string? Description { get; private set; }

        List<Row> Rows => Cells.Where(p=>);

        List<Column> Columns = [];

        List<Cell> Cells = [];

        public DataTable(ID id, string name)
        {
            ID = id;
            Name = name;
        }

        public DataTable(string name)
        {
            ID = TsidCreator.GetTsid();
            Name = name;
        }
    }
}
