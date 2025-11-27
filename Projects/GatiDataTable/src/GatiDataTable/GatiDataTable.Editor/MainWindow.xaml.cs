using GatiDataTable.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GatiDataTable.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly DataTableModel _coreTable;
        public ObservableCollection<GenericRowViewModel> Rows
        {
            get => _rows;
            set
            {
                _rows = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<GenericRowViewModel> _rows = [];

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var schema = new DataTableSchema("Monster")
                .AddColumn("Id", ColumnKind.Int)
                .AddColumn("Name", ColumnKind.String)
                .AddColumn("Hp", ColumnKind.Float)
                .AddColumn("IsBoss", ColumnKind.Bool);

            _coreTable = new DataTableModel(schema);

            var r1 = _coreTable.AddRow();
            r1.Set("Id", 1);
            r1.Set("Name", "Slime");
            r1.Set("Hp", 50f);
            r1.Set("IsBoss", false);

            var r2 = _coreTable.AddRow();
            r2.Set("Id", 2);
            r2.Set("Name", "Orc");
            r2.Set("Hp", 120f);
            r2.Set("IsBoss", true);

            Rows = [.. _coreTable.Rows.Select(row => new GenericRowViewModel(row))];

            Rows.CollectionChanged += Rows_CollectionChanged;

            // 스키마를 보고 DataGrid 컬럼을 동적으로 생성
            BuildColumns(schema);
        }

        private void BuildColumns(DataTableSchema schema)
        {
            DataGrid.Columns.Clear();

            foreach (var col in schema.Columns)
            {
                switch (col.Kind)
                {
                    case ColumnKind.Bool:
                        {
                            // 체크박스 컬럼
                            var boolColumn = new DataGridCheckBoxColumn
                            {
                                Header = col.Name,
                                Binding = new Binding($"[{col.Name}]")
                                {
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                }
                            };

                            // 2-state 체크박스
                            var elementStyle = new Style(typeof(CheckBox));
                            elementStyle.Setters.Add(new Setter(CheckBox.IsThreeStateProperty, false));
                            boolColumn.ElementStyle = elementStyle;
                            boolColumn.EditingElementStyle = elementStyle;

                            DataGrid.Columns.Add(boolColumn);
                        }
                        break;

                    default:
                        // 텍스트 컬럼 (int, float, string, enum도 일단 텍스트)
                        DataGrid.Columns.Add(new DataGridTextColumn
                        {
                            Header = col.Name,
                            Binding = new Binding($"[{col.Name}]")
                            {
                                Mode = BindingMode.TwoWay,
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            }
                        });
                        break;
                }
            }
        }

        private void Rows_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (MonsterRowViewModel vm in e.OldItems)
                {
                    _coreTable.Rows.Remove(GetInnerRow(vm));
                }
            }
        }

        private static DataRowModel GetInnerRow(MonsterRowViewModel vm)
        {
            return vm.GetRow();
        }

        private void OnAddRowClick(object sender, RoutedEventArgs e)
        {
            // 1) Core에 새 Row 생성
            var newRow = _coreTable.AddRow();

            foreach (var col in _coreTable.Schema.Columns)
            {
                switch (col.Kind)
                {
                    case ColumnKind.Byte:
                        {
                            if (col.IsNullable)
                            {
                                newRow.Set<byte?>(col.Name, null);
                            }
                            else
                            {
                                newRow.Set<byte>(col.Name, 0);
                            }
                        }
                        break;
                    case ColumnKind.Short:
                        {
                            if (col.IsUnsigned)
                            {
                                if (col.IsNullable)
                                {
                                    newRow.Set<ushort?>(col.Name, null);
                                }
                                else
                                {
                                    newRow.Set<ushort>(col.Name, 0);
                                }
                            }
                            else
                            {
                                if (col.IsNullable)
                                {
                                    newRow.Set<short?>(col.Name, null);
                                }
                                else
                                {
                                    newRow.Set<short>(col.Name, 0);
                                }
                            }
                        }
                        break;
                    case ColumnKind.Int:
                        {
                            if (col.IsUnsigned)
                            {
                                if (col.IsNullable)
                                {
                                    newRow.Set<uint?>(col.Name, null);
                                }
                                else
                                {
                                    newRow.Set<uint>(col.Name, 0);
                                }
                            }
                            else
                            {
                                if (col.IsNullable)
                                {
                                    newRow.Set<int?>(col.Name, null);
                                }
                                else
                                {
                                    newRow.Set<int>(col.Name, 0);
                                }
                            }
                        }
                        break;
                    case ColumnKind.Float:
                        {
                            if (col.IsNullable)
                            {
                                newRow.Set<float?>(col.Name, null);
                            }
                            else
                            {
                                newRow.Set<float>(col.Name, 0f);
                            }
                        }
                        break;
                    case ColumnKind.Double:
                        {
                            if (col.IsNullable)
                            {
                                newRow.Set<double?>(col.Name, null);
                            }
                            else
                            {
                                newRow.Set<double>(col.Name, 0);
                            }
                        }
                        break;
                    case ColumnKind.Bool:
                        {
                            if (col.IsNullable)
                            {
                                newRow.Set<bool?>(col.Name, null);
                            }
                            else
                            {
                                newRow.Set<bool>(col.Name, false);
                            }
                        }
                        break;
                    case ColumnKind.String:
                        {
                            if (col.IsNullable)
                            {
                                newRow.Set<string?>(col.Name, null);
                            }
                            else
                            {
                                newRow.Set<string>(col.Name, string.Empty);
                            }
                        }
                        break;
                    default:
                        newRow.Set(col.Name, string.Empty);
                        break;
                }
            }

            // 2) ViewModel로 감싸서 DataGrid에 추가
            var vm = new GenericRowViewModel(newRow);
            Rows.Add(vm);
        }
    }
}