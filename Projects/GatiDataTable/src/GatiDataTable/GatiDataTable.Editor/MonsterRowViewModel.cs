using GatiDataTable.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GatiDataTable.Editor
{
    public sealed class MonsterRowViewModel : INotifyPropertyChanged
    {
        private readonly DataRowModel _inner;

        public MonsterRowViewModel(DataRowModel inner)
        {
            _inner = inner;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get
            {
                return _inner.Get<int>(nameof(Id));
            }
            set
            {
                _inner.Set(nameof(Id), value);
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _inner.Get<string>(nameof(Name)) ?? string.Empty;
            }
            set
            {
                _inner.Set(nameof(Name), value);
                OnPropertyChanged();
            }
        }

        public float Hp
        {
            get
            {
                return _inner.Get<float>(nameof(Hp));
            }
            set
            {
                _inner.Set(nameof(Hp), value);
                OnPropertyChanged();
            }
        }

        public DataRowModel GetRow() => _inner;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}