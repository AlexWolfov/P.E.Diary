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
    /// Interaction logic for TestParticipatorsTable.xaml
    /// </summary>
    public partial class TestParticipatorsTable : UserControl
    {
        public TestParticipatorsTable()
        {
            InitializeComponent();
        }
    }
    
    public class TestParticipator
    {
        public TestParticipator(string surname, string name, int mark)
        {
            Фамилия = surname;
            Имя = name;
            Оценка = mark;
        }

        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public int Оценка { get; set; }
    }
}
