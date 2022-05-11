using System.Windows;

namespace P.E.Diary
{
    /// <summary>
    /// Interaction logic for NormativesForm.xaml
    /// </summary>
    public partial class NormativesForm : Window
    {
        public NormativesForm()
        {
            InitializeComponent();
            LeftMenu.NormativesList = NormativesList;
            Show();
        }
    }
}
