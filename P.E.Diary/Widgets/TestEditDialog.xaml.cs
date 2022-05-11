using System.Collections.Generic;
using System.Windows;

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
