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
