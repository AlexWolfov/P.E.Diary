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
                PupilsTable.ClearTable();
            }
        }


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
    }
}
