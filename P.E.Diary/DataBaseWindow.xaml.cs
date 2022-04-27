using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace P.E.Diary
{
    /// <summary>
    /// Interaction logic for DataBaseWindow.xaml
    /// </summary>
    public partial class DataBaseWindow : Window
    {
        public DataBaseWindow()
        {
            InitializeComponent();
            Show();
        }

        private void ReplaceDataBase()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string dataBasePath = openFileDialog.FileName;
            File.Copy(".\\Database", ".\\Database save.old", true);
            File.Copy(dataBasePath, ".\\Database", true);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            ReplaceDataBase();
            MessageBox.Show("Приложение должно быть перезапущено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Shutdown();
        }
    }
}
