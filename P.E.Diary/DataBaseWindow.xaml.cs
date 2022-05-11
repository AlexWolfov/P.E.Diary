using Microsoft.Win32;
using System.IO;
using System.Windows;

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

        private bool ReplaceDataBase() //tue если заменено
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string dataBasePath = openFileDialog.FileName;
            if (dataBasePath != "")
            {
                File.Copy(".\\Database", ".\\Database save.old", true);
                File.Copy(dataBasePath, ".\\Database", true);
                return true;
            }
            return false;
        }

        private bool SaveDataBase()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();
            string dataBasePath = saveFileDialog.FileName;
            if (dataBasePath != "")
            {
                File.Copy(".\\Database", dataBasePath, true);
                return true;
            }
            return false;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveDataBase())
            {
                FoolProof.UniversalProtection("Неверный путь");
            }

        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (ReplaceDataBase())
            {
                MessageBox.Show("Приложение должно быть перезапущено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Shutdown();
            }
            else
            {
                FoolProof.UniversalProtection("Неверный путь");
            }
        }
    }
}
