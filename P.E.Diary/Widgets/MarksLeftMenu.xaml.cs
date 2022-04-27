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
    /// Interaction logic for MarksLeftMenu.xaml
    /// </summary>
    public partial class MarksLeftMenu : UserControl
    {
        private Dictionary<string, SchoolClass> _classes;
        public PupilsMarksTable PupilsTable = new PupilsMarksTable();

        public MarksLeftMenu()
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

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (PupilsTable.Table.CurrentCell != null)
            {
                TestEditDialog testEditDialog = new TestEditDialog(_classes[ClassesList.SelectedItem.ToString()].Pupils,
                    PupilsTable.GetSelectedTest());
            }
            else
            {
                FoolProof.UniversalProtection("Вы не выбрали зачет");
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClassesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassesList.SelectedItem != null)
            {
                PupilsTable.LoadTable(_classes[ClassesList.SelectedItem.ToString()]);
            }
        }
    }
}
