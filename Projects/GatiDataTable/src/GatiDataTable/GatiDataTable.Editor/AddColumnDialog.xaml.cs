using GatiDataTable.Core;
using System;
using System.Collections.Generic;
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
    /// AddColumnDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddColumnDialog : Window
    {
        // 다이얼로그에서 읽어갈 공개 프로퍼티들
        public string ColumnName => TxtName.Text.Trim();

        public ColumnKind SelectedKind =>
            CmbType.SelectedItem is ColumnKind kind ? kind : ColumnKind.String;

        private readonly DataTableSchema _schema;

        public string DefaultValueText => TxtDefault.Text.Trim();

        public string EnumTypeName => TxtEnumType.Text.Trim();

        public AddColumnDialog(DataTableSchema schema)
        {
            _schema = schema;

            InitializeComponent();

            CmbType.ItemsSource = Enum.GetValues<ColumnKind>();
            CmbType.SelectedItem = ColumnKind.Int;
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ColumnName))
            {
                MessageBox.Show(this,
                    "컬럼명을 입력해주세요.",
                    "검증 오류",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TxtName.Focus();
                return;
            }

            // 필요하면 여기에서 컬럼명 규칙(알파벳/숫자만 허용 등) 검증도 추가 가능

            DialogResult = true;
            Close();
        }
    }
}
