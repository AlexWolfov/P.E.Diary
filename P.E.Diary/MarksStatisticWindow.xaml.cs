using System.Windows;

namespace P.E.Diary
{
    /// <summary>
    /// Interaction logic for MarksStatisticWindow.xaml
    /// </summary>
    public partial class MarksStatisticWindow : Window
    {
        public MarksStatisticWindow()
        {
            InitializeComponent();
            LeftMenu.PupilsTable = MarksTable;
            Show();
        }
    }
}
