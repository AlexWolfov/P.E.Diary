using P.E.Diary.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for EditTestParticipatorsTable.xaml
    /// </summary>
    public partial class EditTestParticipatorsTable : UserControl
    {
        private List<Pupil> _pupils;
        private Test _currentTest;

        public EditTestParticipatorsTable()
        {
            InitializeComponent();
        }

        public void EditTest()
        {
            for (int i = 0; i < _pupils.Count; i++)
            {
                SqlReader.EditTest(_currentTest.Normative.Id, _pupils[i].Id,
                    _currentTest.Date, Convert.ToDouble(((TestParticipator)Table.Items[i]).Результат));
            }
        }

        public void LoadData(List<Pupil> pupils, Test test)
        {
            _pupils = pupils;
            _currentTest = test;
            List<TestParticipator> participators = new List<TestParticipator>();
            foreach (Pupil pupil in _pupils)
            {
                participators.Add(new TestParticipator(pupil.Surname, pupil.Name,
                    SqlReader.GetTestResult(_currentTest.Normative.Id, pupil.Id, _currentTest.Date)));

            }
            Table.ItemsSource = participators;
        }
    }
}
