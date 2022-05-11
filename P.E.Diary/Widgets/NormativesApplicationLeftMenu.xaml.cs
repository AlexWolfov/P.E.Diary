using System.Windows;
using System.Windows.Controls;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for NormativesApplicationLeftMenu.xaml
    /// </summary>
    public partial class NormativesApplicationLeftMenu : UserControl
    {
        public TestParticipatorsTable TestParticipatorsTable;
        public NormativesList NormativesList;
        public ApplyNormativeWindow Window;

        public NormativesApplicationLeftMenu()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestParticipatorsTable.ApplyNormative(NormativesList.GetActiveNormative()))
            {
                Window.Close();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            TestParticipatorsTable.DeleteSelectedRow();
        }
    }
}
