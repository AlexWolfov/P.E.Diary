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
            TestParticipatorsTable.ApplyNormative(NormativesList.GetActiveNormative());
            Window.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            TestParticipatorsTable.DeleteSelectedRow();
        }
    }
}
