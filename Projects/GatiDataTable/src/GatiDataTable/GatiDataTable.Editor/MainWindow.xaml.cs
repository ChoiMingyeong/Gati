using GatiDataTable.Core;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
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
        private ObservableCollection<DataTableEntry> _tables = [];
        public ObservableCollection<DataTableEntry> Tables
        {
            get => _tables;
            set
            {
                _tables = value;
                OnPropertyChanged();
            }
        }

        private DataTableEntry? _selectedTable;
        public DataTableEntry? SelectedTable
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
                .AddColumn("Name", ColumnKind.String)
                .AddColumn("Hp", ColumnKind.Float)
                .AddColumn("IsBoss", ColumnKind.Bool);

            var monsterTable = new DataTableModel(monsterSchema);

            var mr1 = monsterTable.AddRow();
            mr1.Set("Name", "Slime");
            mr1.Set("Hp", 50f);
            mr1.Set("IsBoss", false);

            var mr2 = monsterTable.AddRow();
            mr2.Set("Name", "Orc");
            mr2.Set("Hp", 120f);
            mr2.Set("IsBoss", false);

            // Skill 테이블 (예시)
            var skillSchema = new DataTableSchema("Skill")
                .AddColumn("Name", ColumnKind.String)
                .AddColumn("MpCost", ColumnKind.Int);

            var skillTable = new DataTableModel(skillSchema);

            var sr1 = skillTable.AddRow();
            sr1.Set("Name", "Fire Ball");
            sr1.Set("MpCost", 10);

            var sr2 = skillTable.AddRow();
            sr2.Set("Name", "Ice Spear");
            sr2.Set("MpCost", 15);

            Tables =
            [
                new DataTableEntry(monsterTable),
                new DataTableEntry(skillTable),
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
                DataGridColumn column;

                switch (col.Kind)
                {
                    case ColumnKind.Bool:
                        {
                            // 체크박스 컬럼
                            column = new DataGridCheckBoxColumn
                            {
                                Header = col.Name,
                                Binding = new Binding($"[{col.Name}]")
                                {
                                    Mode = BindingMode.TwoWay,
                                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                }
                            };
                        }
                        break;

                    default:
                        // 텍스트 컬럼 (int, float, string, enum도 일단 텍스트)
                        column = new DataGridTextColumn
                        {
                            Header = col.Name,
                            Binding = new Binding($"[{col.Name}]")
                            {
                                Mode = BindingMode.TwoWay,
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                            }
                        };
                        break;
                }

                // 시스템/읽기전용 컬럼은 수정 불가
                if (col.IsReadOnly || col.IsSystem)
                {
                    column.IsReadOnly = true;
                }

                DataGrid.Columns.Add(column);
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
                    {
                        return i;
                    }
                    return 0;

                case ColumnKind.Float:
                    if (float.TryParse(text, out var f))
                    {
                        return f;
                    }
                    return 0f;

                case ColumnKind.Bool:
                    if (bool.TryParse(text, out var b))
                    {
                        return b;
                    }

                    // "0", "1" 같은 케이스를 처리하고 싶다면:
                    if (text == "0")
                    {
                        return false;
                    }
                    if (text == "1")
                    {
                        return true;
                    }

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

        private void OnAddTableClick(object sender, RoutedEventArgs e)
        {
            var dlg = new AddTableDialog(Tables)
            {
                Owner = this
            };

            if (dlg.ShowDialog() == true)
            {
                string name = dlg.TableName;

                var schema = new DataTableSchema(name);
                var table = new DataTableModel(schema);
                var entry = new DataTableEntry(table);
                Tables.Add(entry);

                SelectedTable = entry;
            }
        }

        private void OnDeleteTableClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTable is null)
            {
                return;
            }

            if (MessageBox.Show(this,
                    $"테이블 '{SelectedTable.Name}'을(를) 정말 삭제할까요?\n해당 테이블의 모든 데이터도 삭제됩니다.",
                    "테이블 삭제 확인",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes)
            {
                return;
            }

            var toRemove = SelectedTable;
            int idx = Tables.IndexOf(toRemove);

            Tables.Remove(toRemove);

            // 삭제 후 선택 상태 갱신
            if (Tables.Count == 0)
            {
                SelectedTable = null;
                Rows = new ObservableCollection<GenericRowViewModel>();
                DataGrid.Columns.Clear();
            }
            else
            {
                if (idx >= Tables.Count)
                {
                    idx = Tables.Count - 1;
                }
                SelectedTable = Tables[idx];
            }
        }
    }
}