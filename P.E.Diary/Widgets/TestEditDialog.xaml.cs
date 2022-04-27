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

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for TestEditDialog.xaml
    /// </summary>
    public partial class TestEditDialog : Window
    {
        public TestEditDialog(List<Pupil> pupils, Test test)
        {
            InitializeComponent();
            EditTestTable.LoadData(pupils, test);
            TestInfo.Text = test.Normative.Name + " " + test.Date;
            Show();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditTestTable.EditTest();
            Close();
        }
    }
}
