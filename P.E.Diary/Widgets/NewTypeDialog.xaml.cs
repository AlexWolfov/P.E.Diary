using System;
using System.Windows;

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

        private void CreateType_Click(object sender, EventArgs e)
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
