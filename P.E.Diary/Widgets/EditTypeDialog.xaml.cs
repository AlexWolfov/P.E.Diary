using System.Windows;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for EditTypeDialog.xaml
    /// </summary>
    public partial class EditTypeDialog : Window
    {
        private string _oldName;
        private EditNormativeDialog _normativeForm;
        private NormativesList _normativesList;

        public EditTypeDialog(NormativesList normativesList, string oldName)
        {
            _normativesList = normativesList;
            _oldName = oldName;
            InitializeComponent();
            AddOldData();
            Show();
        }

        public void AddOldData() //вставляем старые данные
        {
            NameBox.Text = _oldName;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text != "")
            {
                SqlReader.EditType(_oldName, NameBox.Text);
                _normativesList.LoadNormatives();
                if (!(_normativeForm is null))
                {
                    _normativeForm.LoadTypes();
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
