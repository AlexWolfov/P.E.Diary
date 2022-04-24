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
    /// Interaction logic for NewNormativeDialog.xaml
    /// </summary>
    public partial class NewNormativeDialog : Window
    {
        public NewNormativeDialog()
        {
            InitializeComponent();
        }

        private void CreateNewNormative()
        {
            Normative normative = new Normative();
            normative.Name = NameBox.Text;
            normative.Formula = Formula.Text;
            if (TypeSelection.SelectedItem != null)
            {
                normative.Type = TypeSelection.SelectedItem.ToString();
            }
            else
            {
                normative.Type = "Без категории";
            }
            SqlReader.CreateNormative(normative);
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

        private void Create_Click(object sender, EventArgs e)
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

        private void NewType_Click(object sender, EventArgs e)
        {
            NewTypeDialog newTypeForm = new NewTypeDialog(this);
            newTypeForm.Show();
        }

        private void DeleteType_Click(object sender, EventArgs e)
        {
            if (TypeSelection.SelectedItem != null)
            {
                if (FoolProof.DeletionProtection(TypeSelection.SelectedItem.ToString()));
                {
                    SqlReader.DeleteType(TypeSelection.SelectedItem.ToString());
                    SqlReader.LoadNormatives();
                    LoadTypes();
                }
            }
            else
            {
                FoolProof.UniversalProtection("Нельзя удалить отсутствующую категорию");
            }
        }
    }
}
