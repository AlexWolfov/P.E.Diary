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
    /// Interaction logic for NewNromativeDialog.xaml
    /// </summary>
    public partial class NewNromativeDialog : Window
    {
        public NewNromativeDialog()
        {
            InitializeComponent();
        }


        private void CreateNormative()
        {
            Normative normative = new Normative();
            normative.Name = NameBox.Text;
            normative.Formula = Formula.Text;
            if (Types.SelectedItem != null)
            {
                normative.Type = Types.SelectedItem.ToString();
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
            if ((Types != null))
            {
                Types.Items.Clear();
            }
            Types.Items.AddRange(SqlReader.LoadTypes().ToArray()); //загружаем список типов
        }

        private void Create_Click(object sender, EventArgs e)
        {
            if (NameBox.Text != "" && Formula.Text != "")
            {
                CreateNormative();
                Close();
            }
            else
            {
                FoolProof.UniversalProtection("Заполните имя и формулу");
            }
        }

        private void NewType_Click(object sender, EventArgs e)
        {
            NewType newTypeForm = new NewType(this);
            newTypeForm.Show();
        }

        private void DeleteType_Click(object sender, EventArgs e)
        {
            if (Types.SelectedItem != null)
            {
                SqlReader.DeleteType(Types.SelectedItem.ToString());
                SqlReader.LoadNormatives();
                LoadTypes();
            }
            else
            {
                FoolProof.UniversalProtection("Нельзя удалить отсутствующую категорию");
            }
        }
    }
}
