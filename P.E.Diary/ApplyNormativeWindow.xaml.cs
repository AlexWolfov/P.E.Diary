using System.Collections.Generic;
using System.Windows;

namespace P.E.Diary
{
    /// <summary>
    /// Interaction logic for ApplyNormativeWindow.xaml
    /// </summary>
    public partial class ApplyNormativeWindow : Window
    {

        public ApplyNormativeWindow(List<Pupil> pupils)
        {
            InitializeComponent();
            Table.LoadData(pupils);
            LeftMenu.TestParticipatorsTable = Table;
            LeftMenu.NormativesList = NormativesList;
            LeftMenu.Window = this;
            NormativesList.TestParticipatorsTable = Table;
            NormativesList.ParentWindow = this;
            Show();
        }
    }
}
