using P.E.Diary.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            return Convert.ToDouble(((TestParticipator)Table.Items[index]).Результат);
        }

        public void LoadData(List<Pupil> pupils)
        {
            _pupils = pupils;
            List<TestParticipator> participators = new List<TestParticipator>();
            foreach (Pupil pupil in _pupils)
            {
                participators.Add(new TestParticipator(pupil, 0));
            }
            Table.ItemsSource = participators;
        }

        public bool ApplyNormative(Normative normative) //true если удалось провести зачет
        {
            if (normative != null)
            {
                for (int i = 0; i < _pupils.Count; i++)
                {
                    SqlReader.ApplyNormative(normative.Id,
                        _pupils[i].Id, //применяем норматив для данного ученика
                        ReturnResultByIndex(i));
                }
                return true;
            }
            else
            {
                FoolProof.UniversalProtection("Выберите норматив");
            }
            return false;
        }

        public void DeleteSelectedRow()
        {
            if (Table.SelectedItem != null)
            {
                if (FoolProof.MessageBoxProtection("Вы уверены, что хотите убрать ученика (учеников) с тренировки?", "Удаление"))
                {
                    foreach (TestParticipator participator in Table.SelectedItems)
                    {
                        _pupils.Remove(participator.Pupil);
                        ((List<TestParticipator>)Table.ItemsSource).Remove(participator);
                    }
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
}
