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

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for NewClassDialog.xaml
    /// </summary>
    public partial class NewClassDialog : Window
    {
        public MainWindow MainWindow;

        public NewClassDialog()
        {
            InitializeComponent();
            Show();
        }

        private void CreateClass_Click(object sender, RoutedEventArgs e)
        {
            if (NumberBox.Text != "" && LetterBox.Text != "")
            {
                SqlReader.NewClass(Convert.ToInt32(NumberBox.Text),
                    LetterBox.Text); //добавляем школьный класс с нашими данными в БД
                string key = NumberBox.Text + " " + LetterBox.Text;
                MainWindow.LeftMenu.AddClass(new SchoolClass(Convert.ToInt32(NumberBox.Text), LetterBox.Text,
                    new List<Pupil>())); //добавляем наш школьный класс в основную форму
                MainWindow.LeftMenu.LoadClasses();
                Close();
            }
            else
            {
                FoolProof.UniversalProtection("Заполните поля");
            }
        }

        private void NumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int num = 0;
            FoolProof.SetInt(ref num, NumberBox.Text);
            NumberBox.Text = num.ToString();
        }
    }
}
