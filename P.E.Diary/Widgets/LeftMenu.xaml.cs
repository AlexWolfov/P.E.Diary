using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for LeftMenu.xaml
    /// </summary>
    public partial class LeftMenu : UserControl
    {
        public MainWindow MainWindow;
        public PupilsTable PupilsTable;
        private Dictionary<string, SchoolClass> _classes;

        public LeftMenu()
        {
            InitializeComponent();
            LoadClasses();
        }

        #region Back

        public void LoadClasses()
        {
            _classes = SqlReader.LoadClasses();
            ClassesList.Items.Clear();
            foreach (string key in _classes.Keys)
            {
                ClassesList.Items.Add(key);
            }
        }

        public void AddClass(SchoolClass schoolClass)  //добавляем школьный класс в соновную форму
        {
            _classes.Add(schoolClass.Grade.ToString() + ' ' + schoolClass.Letter, schoolClass);
        }

        private void DeleteCurrentClass()
        {
            if (FoolProof.DeletionProtection(ClassesList.SelectedItem.ToString()))
            {
                var classInfo = ClassesList.SelectedItem.ToString().Split(' '); //номер и буква класса
                SqlReader.DeleteClass(Convert.ToInt32(classInfo[0]), classInfo[1]);
                _classes.Remove(classInfo[0] + ' ' + classInfo[1]); //Удаляем из словаря класс с текущим ключем
                ClassesList.Items.Remove(ClassesList.SelectedItem);
                ClassesList.Items.Refresh();
            }
        }

        #endregion

        #region Events

        private void ClassesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassesList.SelectedItem != null)
            {
                PupilsTable.LoadTable(_classes[ClassesList.SelectedItem.ToString()]);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewClassDialog newClassDialog = new NewClassDialog(MainWindow);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteCurrentClass();
        }

        private void Normatives_Click(object sender, RoutedEventArgs e)
        {
            NormativesForm normativesForm = new NormativesForm();
        }

        private void MarksStatisticButton_Click(object sender, RoutedEventArgs e)
        {
            MarksStatisticWindow marksStatisticWindow = new MarksStatisticWindow();
        }

        private void ApplyNormative_Click(object sender, RoutedEventArgs e)
        {
            List<Pupil> selectedPupils = PupilsTable.ReturnSelectedPupils();
            if (selectedPupils.Count > 0)
            {
                ApplyNormativeWindow applyNormativeWindow = new ApplyNormativeWindow(selectedPupils);
            }
            else
            {
                FoolProof.UniversalProtection("Выберите учеников");
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseWindow dataBaseWindow = new DataBaseWindow();
        }
        #endregion

    }
}
