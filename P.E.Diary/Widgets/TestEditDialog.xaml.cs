using System.Collections.Generic;
using System.Windows;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for TestEditDialog.xaml
    /// </summary>
    public partial class TestEditDialog : Window
    {
        PupilsMarksTable _table;
        public TestEditDialog(List<Pupil> pupils, Test test, PupilsMarksTable table)
        {
            InitializeComponent();
            EditTestTable.LoadData(pupils, test);
            TestInfo.Text = test.Normative.Name + " " + test.Date;
            _table = table;
            Show();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditTestTable.EditTest();
            _table.LoadTable(_table.CurrentSchoolClass);
            Close();
        }
    }
}
