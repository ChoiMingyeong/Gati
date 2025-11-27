using GatiDataTable.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GatiDataTable.Editor
{
    public class GenericRowViewModel : INotifyPropertyChanged
    {
        private readonly DataRowModel _row;

        public GenericRowViewModel(DataRowModel row)
        {
            _row = row;
        }

        public object? this[string columnName]
        {
            get => _row[columnName];
            set
            {
                if (!Equals(_row[columnName], value))
                {
                    _row[columnName] = value;
                    OnPropertyChanged("Item[]");
                }
            }
        }

        public DataRowModel Row => _row; // 삭제/저장 시 Core에 접근할 때 사용

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}