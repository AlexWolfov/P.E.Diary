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
        private List<Pupil> _pupils;

        public TestParticipatorsTable()
        {
            InitializeComponent();
        }

        private int GetCurrentCellAdressAxesY()
        {
            return Table.SelectedIndex;
        }

        private int GetCurrentCellAdressAxesX()
        {
            return Table.CurrentCell.Column.DisplayIndex;
        }
        
        private double ReturnResultByIndex(int index)
        {
            return ((TestParticipator) Table.Items[index]).Результат;
        }

        public void LoadData(List<Pupil> pupils)
        {
            _pupils = pupils;
            List<TestParticipator> participators = new List<TestParticipator>();
            foreach (Pupil pupil in _pupils)
            {
                participators.Add(new TestParticipator(pupil.Surname, pupil.Name, 0));
            }
            Table.ItemsSource = participators;
        }

        public void ApplyNormative(Normative normative)
        {
            if (normative != null)
            {
                for (int i = 0; i < _pupils.Count; i++)
                {
                    SqlReader.ApplyNormative(normative.Id,
                        _pupils[i].Id, //применяем норматив для данного ученика
                        ReturnResultByIndex(i));
                }
            }
            else
            {
                FoolProof.UniversalProtection("Выберите норматив");
            }
        }

        public void DeleteSelectedRow()
        {
            if (Table.SelectedItem != null)
            {
                if (FoolProof.MessageBoxProtection("Вы уверены, что хотите убрать ученика с тренировки?", "Удаление"))
                {
                    _pupils.RemoveAt(GetCurrentCellAdressAxesY());
                    ((List<TestParticipator>)Table.ItemsSource).Remove((TestParticipator)Table.SelectedItem);
                    Table.Items.Refresh();
                }
            }
            else
            {
                FoolProof.UniversalProtection("Выберите ученика");
            }
        }

        #region Events

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedRow();
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (Table.SelectedItem == null)
            {
                foreach (MenuItem item in ContextMenu.Items)
                {
                    if (item.Name == "DeleteButton")
                    {
                        item.IsEnabled = false;
                        break;
                    }
                }
            }
            else
            {
                foreach (MenuItem item in ContextMenu.Items)
                {
                    if (item.Name == "DeleteButton")
                    {
                        item.IsEnabled = true;
                        break;
                    }
                }
            }
        }

        private void Table_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            double newValue = 0;
            if (!FoolProof.SetDouble(ref newValue, (e.EditingElement as TextBox).Text))
            {
                e.Cancel = true;
            }
        }

        private void Table_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    if (!FoolProof.MessageBoxProtection("Вы уверены, что хотите убрать ученика с тренировки?", "Удаление"))
                    {
                        e.Handled = true;
                    }
                    break;
                case Key.Escape:
                    Table.SelectedItem = null;
                    break;
                default:
                    break;            
            }
        }

        #endregion

    }

    public class TestParticipator
    {
        public TestParticipator(string surname, string name, double result)
        {
            Фамилия = surname;
            Имя = name;
            Результат = result;
        }

        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public double Результат { get; set; }
    }
}
