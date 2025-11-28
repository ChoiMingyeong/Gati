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
        private ObservableCollection<DatatTableEntry> _tables;
        public ObservableCollection<DatatTableEntry> Tables
        {
            get => _tables;
            set
            {
                _tables = value;
                OnPropertyChanged();
            }
        }

        private DatatTableEntry? _selectedTable;
        public DatatTableEntry? SelectedTable
        {
            get => _selectedTable;
            set
            {
                if (_selectedTable != value)
                {
                    _selectedTable = value;
                    OnPropertyChanged();
                    OnSelectedTableChanged();
                }
            }
        }

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

            InitializeTables();


            Rows.CollectionChanged += Rows_CollectionChanged;
        }

        private void InitializeTables()
        {
            // Monster 테이블
            var monsterSchema = new DataTableSchema("Monster")
                .AddColumn("Id", ColumnKind.Int)
                .AddColumn("Name", ColumnKind.String)
                .AddColumn("Hp", ColumnKind.Float)
                .AddColumn("IsBoss", ColumnKind.Bool);

            var monsterTable = new DataTableModel(monsterSchema);

            var mr1 = monsterTable.AddRow();
            mr1.Set("Id", 1);
            mr1.Set("Name", "Slime");
            mr1.Set("Hp", 50f);
            mr1.Set("IsBoss", false);

            var mr2 = monsterTable.AddRow();
            mr2.Set("Id", 2);
            mr2.Set("Name", "Orc");
            mr2.Set("Hp", 120f);
            mr2.Set("IsBoss", false);

            // Skill 테이블 (예시)
            var skillSchema = new DataTableSchema("Skill")
                .AddColumn("Id", ColumnKind.Int)
                .AddColumn("Name", ColumnKind.String)
                .AddColumn("MpCost", ColumnKind.Int);

            var skillTable = new DataTableModel(skillSchema);

            var sr1 = skillTable.AddRow();
            sr1.Set("Id", 100);
            sr1.Set("Name", "Fire Ball");
            sr1.Set("MpCost", 10);

            var sr2 = skillTable.AddRow();
            sr2.Set("Id", 101);
            sr2.Set("Name", "Ice Spear");
            sr2.Set("MpCost", 15);

            Tables =
            [
                new DatatTableEntry(monsterTable),
                new DatatTableEntry(skillTable),
            ];

            SelectedTable = Tables.FirstOrDefault();
        }

        private void OnSelectedTableChanged()
        {
            Rows.CollectionChanged -= Rows_CollectionChanged;

            if (SelectedTable is null)
            {
                Rows = [];
                DataGrid.Columns.Clear();
                return;
            }

            var table = SelectedTable.Table;
            Rows = [.. table.Rows.Select(r => new GenericRowViewModel(r))];
            Rows.CollectionChanged += Rows_CollectionChanged;

            BuildColumns(table.Schema);
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
                            DataGrid.Columns.Add(new DataGridCheckBoxColumn
                            {
                                Header = col.Name,
                                Binding = new Binding($"[{col.Name}]")
                                {
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                }
                            });
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
            if(SelectedTable is null)
            {
                return;
            }

            var table = SelectedTable.Table;

            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (GenericRowViewModel vm in e.OldItems)
                {
                    table.Rows.Remove(GetInnerRow(vm));
                }
            }
        }

        private static DataRowModel GetInnerRow(GenericRowViewModel vm)
        {
            return vm.Row;
        }

        private void OnAddRowClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTable is null)
                return;

            var table = SelectedTable.Table;
            var newRow = table.AddRow();

            foreach (var col in table.Schema.Columns)
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

            Rows.Add(new GenericRowViewModel(newRow));
        }

        private void OnAddColumnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTable is null)
                return;

            var table = SelectedTable.Table;
            var dlg = new AddColumnDialog(table.Schema)
            {
                Owner = this
            };

            if (dlg.ShowDialog() == true)
            {
                var name = dlg.ColumnName;
                var kind = dlg.SelectedKind;
                var enumTypeName = dlg.EnumTypeName;
                var defaultObj = ParseDefaultValue(kind, dlg.DefaultValueText);

                table.AddColumn(name, kind, enumTypeName, defaultValue: defaultObj);

                BuildColumns(table.Schema);
            }
        }

        private static object? ParseDefaultValue(ColumnKind kind, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                // 비어 있을 때 타입별 기본값
                return kind switch
                {
                    ColumnKind.Int => 0,
                    ColumnKind.Float => 0f,
                    ColumnKind.Bool => false,
                    ColumnKind.String => string.Empty,
                    _ => null
                };
            }

            switch (kind)
            {
                case ColumnKind.Int:
                    if (int.TryParse(text, out var i))
                        return i;
                    return 0;

                case ColumnKind.Float:
                    if (float.TryParse(text, out var f))
                        return f;
                    return 0f;

                case ColumnKind.Bool:
                    if (bool.TryParse(text, out var b))
                        return b;
                    // "0", "1" 같은 케이스를 처리하고 싶다면:
                    if (text == "0") return false;
                    if (text == "1") return true;
                    return false;

                case ColumnKind.String:
                default:
                    return text;
            }
        }

        private void OnManageColumnsClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTable is null)
                return;

            var table = SelectedTable.Table;

            //var dlg = new ManageColumnsDialog(table)
            //{
            //    Owner = this
            //};

            //dlg.ShowDialog();

            BuildColumns(table.Schema);
        }
    }
}