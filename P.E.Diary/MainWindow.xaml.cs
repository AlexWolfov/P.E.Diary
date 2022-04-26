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
using System.IO;

namespace P.E.Diary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if (!File.Exists(".\\Database"))
            {
                SqlReader.CreateDataBase();
            }
            else
            {
                File.Copy(".\\Database", ".\\Database save", true);
            }
            InitializeComponent();
            LeftMenu.PupilsTable = PupilsTable;
            LeftMenu.MainWindow = this;
        }
    }
}
