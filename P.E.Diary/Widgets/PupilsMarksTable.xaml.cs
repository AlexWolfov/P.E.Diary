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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for PupilsMarksTable.xaml
    /// </summary>
    public partial class PupilsMarksTable : UserControl
    {
        private SchoolClass _currentSchoolClass;
        private List<Test> _tests;

        public PupilsMarksTable()
        {
            InitializeComponent();
        }

        public Test GetSelectedTest()
        {
            if (Table.SelectedCells[0].Column != null)
            {
                return _tests[Table.SelectedCells[0].Column.DisplayIndex - 2];
            }
            return null;
        }

        public void LoadTable(SchoolClass schoolClass)
        {
            _currentSchoolClass = schoolClass;
            _tests = SqlReader.GetSchoolClassTests(schoolClass);
            Table.ItemsSource = null; //очистка
            Table.Columns.Clear();

            PupilMarksRow[] data = new PupilMarksRow[_currentSchoolClass.Pupils.Count];
            Binding binding = new Binding("Data[0]");
            DataGridTextColumn surnameColumn = new DataGridTextColumn { Header = "Фамилия", Binding = binding };
            Table.Columns.Add(surnameColumn);
            binding = new Binding("Data[1]");
            DataGridTextColumn nameColumn = new DataGridTextColumn { Header = "Имя", Binding = binding };
            Table.Columns.Add(nameColumn);
            for (int i = 0; i < _tests.Count; i++)
            {
                binding = new Binding("Data[" + (i+2) + "]");
                string newColumnHeader = _tests[i].Normative.Name + " " + _tests[i].Date.ToString();
                Table.Columns.Add(new DataGridTextColumn {Header = newColumnHeader, Binding = binding});
            }
            for (int i = 0; i < _currentSchoolClass.Pupils.Count; i++)
            {
                PupilMarksRow row = new PupilMarksRow(_tests.Count + 2);
                row.Data[0] = _currentSchoolClass.Pupils[i].Surname;
                row.Data[1] = _currentSchoolClass.Pupils[i].Name;
                for (int j = 0; j < _tests.Count; j++)
                {
                    string header = _tests[j].Normative.Name + " " + _tests[j].Date.ToString();
                    int mark = _currentSchoolClass.Pupils[i].GetMark(_tests[j].Normative,
                        SqlReader.GetTestResult(_tests[j].Normative.Id, _currentSchoolClass.Pupils[i].Id,
                        _tests[j].Date));
                    if (mark > 5)
                    {
                        row.Data[j + 2] = "5+1";
                    }
                    else
                    {
                        row.Data[j + 2] = mark.ToString();
                    }
                }
                data[i] = row;
            }
            Table.ItemsSource = data;
            Table.Columns.RemoveAt(Table.Columns.Count - 1); //последня строчка - служебная
            Table.Items.Refresh();
        }
    }

    public class PupilMarksRow
    {
        public PupilMarksRow(int length)
        {
            Data = new string[length];
        }
        public string[] Data { get; set; }
    }
}
