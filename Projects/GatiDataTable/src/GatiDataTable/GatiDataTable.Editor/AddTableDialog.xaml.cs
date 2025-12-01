using GatiDataTable.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GatiDataTable.Editor
{
    /// <summary>
    /// AddTableDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddTableDialog : Window
    {
        private readonly ObservableCollection<DataTableEntry> _tables;

        public string TableName => TxtName.Text.Trim();

        public AddTableDialog(ObservableCollection<DataTableEntry> tables)
        {
            _tables = tables;
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(TableName))
            {
                MessageBox.Show(this,
                    "테이블명을 입력해주세요.",
                    "검증 오류",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TxtName.Focus();
                return;
            }

            // 동일 이름 테이블 방지 (대소문자 무시)
            bool exists = _tables.Any(t =>
                string.Equals(t.Name, TableName, StringComparison.OrdinalIgnoreCase));
            if (exists)
            {
                MessageBox.Show(this,
                    "이미 존재하는 테이블명입니다.",
                    "검증 오류",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TxtName.Focus();
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
