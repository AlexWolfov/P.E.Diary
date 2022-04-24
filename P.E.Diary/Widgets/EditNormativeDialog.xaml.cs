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
    /// Interaction logic for EditNormativeDialog.xaml
    /// </summary>
    public partial class EditNormativeDialog : Window
    {

        private NormativesList _normativesList;
        private Normative _editingNormative;

        public EditNormativeDialog(NormativesList normativesList, Normative normative)
        {
            _normativesList = normativesList;
            _editingNormative = normative;
            InitializeComponent();
            LoadTypes();
            InsertData();
            Show();
        }

        private void InsertData()
        {
            NameBox.Text = _editingNormative.Name;
            Formula.Text = _editingNormative.Formula;
        }

        private void CreateNewNormative()
        {
            _editingNormative.Name = NameBox.Text;
            _editingNormative.Formula = Formula.Text;
            if (TypeSelection.SelectedItem != null)
            {
                _editingNormative.Type = TypeSelection.SelectedItem.ToString();
            }
            else
            {
                _editingNormative.Type = "Без категории";
            }
            SqlReader.EditNormative(_editingNormative);
            _normativesList.LoadNormatives();
        }

        public void LoadTypes()
        {
            if ((TypeSelection != null))
            {
                TypeSelection.Items.Clear();
            }
            List<string> typesList = SqlReader.LoadTypes();
            foreach (string typeName in typesList)
            {
                TypeSelection.Items.Add(typeName);
            }
        }

        private void NewType_Click(object sender, EventArgs e)
        {
            NewTypeDialog newTypeForm = new NewTypeDialog(this);
            newTypeForm.Show();
        }

        private void DeleteType_Click(object sender, EventArgs e)
        {
            if (TypeSelection.SelectedItem != null)
            {
                if (FoolProof.DeletionProtection(TypeSelection.SelectedItem.ToString())) ;
                {
                    SqlReader.DeleteType(TypeSelection.SelectedItem.ToString());
                    _normativesList.LoadNormatives();
                    LoadTypes();
                }
            }
            else
            {
                FoolProof.UniversalProtection("Нельзя удалить отсутствующую категорию");
            }
        }

        private void EditNormative_Click(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text != "" && Formula.Text != "")
            {
                CreateNewNormative();
                Close();
            }
            else
            {
                FoolProof.UniversalProtection("Заполните имя и формулу");
            }
        }
    }
}
