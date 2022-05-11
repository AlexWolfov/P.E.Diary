using System.IO;
using System.Windows;

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
