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
    /// Interaction logic for NewTypeDialog.xaml
    /// </summary>
    public partial class NewTypeDialog : Window
    {
        private NewNormativeDialog _newNormativeDialog;
        private EditNormativeDialog _editNormativeDialog;

        public NewTypeDialog(NewNormativeDialog newNormativeDialog)
        {
            _newNormativeDialog = newNormativeDialog;
            InitializeComponent();
        }

        public NewTypeDialog(EditNormativeDialog editNormativeDialog)
        {
            _editNormativeDialog = editNormativeDialog;
            InitializeComponent();
        }

        private void CreateClass_Click(object sender, EventArgs e)
        {
            if (NameBox.Text != "")
            {
                SqlReader.CreateType(NameBox.Text);
                if (!(_newNormativeDialog is null))
                {
                    _newNormativeDialog.LoadTypes();
                }
                else
                {
                    _editNormativeDialog.LoadTypes();
                }
                Close();
            }
            else
            {
                FoolProof.UniversalProtection("Заполните поле");
            }
        }
    }
}
