using System.Collections.Generic;
using System.Windows.Controls;

namespace P.E.Diary.Widgets
{

    //// <summary>
    /// Interaction logic for PupilsTable.xaml
    /// </summary>
    public partial class PupilsTable : UserControl
    {
        public SchoolClass CurrentClass;

        public PupilsTable()
        {
            InitializeComponent();
        }

        #region Back

        public void LoadTable(SchoolClass newClass)
        {
            List<PupilRow> pupilRows = new List<PupilRow>();
            Table.ItemsSource = pupilRows;
            if (!(newClass is null))
            {
                CurrentClass = newClass;
                foreach (var pupil in CurrentClass.Pupils)
                {
                    pupilRows.Add(new PupilRow(pupil));
                }
            }
            Table.Items.Refresh();
        }

        private string GetCurrentCellValue()
        {
            PupilRow selectedItem = (PupilRow)Table.SelectedItem;
            switch (Table.CurrentColumn.Header)
            {
                case "Фамилия":
                    return selectedItem.Фамилия;
                    break;
                case "Имя":
                    return selectedItem.Имя;
                    break;
                case "Пол":
                    return selectedItem.Пол;
                    break;
                case "Дата_рождения":
                    return selectedItem.Дата_рождения;
                    break;
                case "Рост":
                    return selectedItem.Рост;
                    break;
                case "Масса":
                    return selectedItem.Масса;
                    break;
                default:
                    break;
            }
            return "";
        }

        private int GetCurrentCellAdressAxesY()
        {
            return Table.SelectedIndex;
        }

        private int GetCurrentCellAdressAxesX()
        {
            return Table.CurrentCell.Column.DisplayIndex;
        }

        private void EditCellInRow(int rowIndex, int cellIndex, string newData)
        {
            PupilRow selectedItem = (PupilRow)Table.SelectedItem;
            switch (Table.CurrentColumn.Header)
            {
                case "Фамилия":
                    selectedItem.Фамилия = newData;
                    break;
                case "Имя":
                    selectedItem.Имя = newData;
                    break;
                case "Пол":
                    selectedItem.Пол = newData;
                    break;
                case "Дата_рождения":
                    selectedItem.Дата_рождения = newData;
                    break;
                case "Рост":
                    selectedItem.Рост = newData;
                    break;
                case "Масса":
                    selectedItem.Масса = newData;
                    break;
                default:
                    break;
            }
        }

        private void SetSelectedRowValue(string newData)
        {
            int columnIndex = GetCurrentCellAdressAxesX();
            int rowIndex = GetCurrentCellAdressAxesY();
            EditCellInRow(rowIndex, columnIndex, newData);
        }


        private void EditRow(string value, DataGridCellEditEndingEventArgs e)
        {
            if (CurrentClass != null && GetCurrentCellAdressAxesY() != -1)
            {
                try
                {
                    Pupil pupil = CurrentClass.Pupils[GetCurrentCellAdressAxesY()]; //считывем изменяемого ученика
                    switch (e.Column.Header)
                    {
                        case "Фамилия":
                            pupil.Surname = value;
                            break;
                        case "Имя":
                            pupil.Name = value;
                            break;
                        case "Пол":
                            if (value == "М" || value == "Ж")
                            {
                                pupil.Gender = value;
                            }
                            else
                            {
                                FoolProof.UniversalProtection("Введите М либо Ж (русской раскладкой)");
                                e.Cancel = true;
                            }
                            break;
                        case "Дата_рождения":
                            string date = FoolProof.ReturnDate(pupil.Birthday, value);
                            if (date != "-1")
                            {
                                pupil.Birthday = date;
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                            break;
                        case "Рост":
                            if (!FoolProof.SetInt(ref pupil.Height, value))
                            {
                                e.Cancel = true;
                            }
                            break;
                        case "Масса":
                            if (!FoolProof.SetInt(ref pupil.Weight, value))
                            {
                                e.Cancel = true;
                            }
                            break;
                        default:
                            break;
                    }

                    CurrentClass.Pupils[GetCurrentCellAdressAxesY()] = pupil;
                    SqlReader.EditPupil(pupil);
                    //LoadTable(CurrentClass);
                }
                catch { }
            }
        }

        private void AddRow()
        {
            Pupil pupil = new Pupil(
                CurrentClass.Grade, CurrentClass.Letter);
            SqlReader.AddPupil(ref pupil);
            CurrentClass.Pupils.Add(pupil);
            LoadTable(CurrentClass);
        }

        private void DeleteRows()
        {
            foreach (PupilRow pupil in Table.SelectedItems)
            {
                SqlReader.DeletePupil(pupil.Pupil.Id); //удаляем ученика из базы Данных
                CurrentClass.Pupils.Remove(pupil.Pupil);
            }
            LoadTable(CurrentClass);
        }


        private Pupil RowToPupil(int rowX)
        {
            return CurrentClass.Pupils[rowX];

        }

        public List<Pupil> ReturnSelectedPupils()
        {
            if (Table.SelectedItems.Count != 0)
            {
                return CurrentClass.Pupils.GetRange(GetCurrentCellAdressAxesY(), Table.SelectedItems.Count);
            }
            return new List<Pupil>();
        }

        #endregion

        #region Events

        private void Table_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string newValue = (e.EditingElement as TextBox).Text;
            EditRow(newValue, e);
        }

        private void Table_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (CurrentClass != null)
            {
                if (FoolProof.DeletionProtection("ученика"))
                {
                    DeleteRows();
                }
                else
                {
                    Table.Items.Clear();
                }
            }
        }

        private void Table_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (CurrentClass != null)
            {
                if (FoolProof.DeletionProtection("ученика"))
                {
                    DeleteRows();
                }
                else
                {
                    LoadTable(CurrentClass);
                }
            }
        }

        private void DeleteButtonPressed(System.Windows.Input.KeyEventArgs e)
        {
            if (CurrentClass != null)
            {
                if (FoolProof.DeletionProtection("ученика"))
                {
                    DeleteRows();
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void EscapeButtonPressed()
        {
            Table.SelectedItem = null;
        }

        private void Table_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Delete:
                    DeleteButtonPressed(e);
                    break;
                case System.Windows.Input.Key.Escape:
                    EscapeButtonPressed();
                    break;
                default:
                    break;
            }
        }

        private void ContextMenuDeleteAddButtons_Opened(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Table.SelectedItem == null)
            {
                foreach (MenuItem item in ContextMenuDeleteAddButtons.Items)
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
                foreach (MenuItem item in ContextMenuDeleteAddButtons.Items)
                {
                    if (item.Name == "DeleteButton")
                    {
                        item.IsEnabled = true;
                        break;
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CurrentClass != null)
            {
                if (FoolProof.DeletionProtection("ученика"))
                {
                    DeleteRows();
                }
            }
            else
            {
                FoolProof.UniversalProtection("Выберите ученика");
            }
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CurrentClass != null)
            {
                AddRow();
            }
            else
            {
                FoolProof.UniversalProtection("Выберите класс");
            }
        }

        private void ReloadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadTable(CurrentClass);
        }

        #endregion
    }

    public class PupilRow
    {
        public PupilRow(Pupil pupil)
        {
            Фамилия = pupil.Surname;
            Имя = pupil.Name;
            Пол = pupil.Gender;
            Дата_рождения = pupil.Birthday.Substring(0,10);
            Рост = pupil.Height.ToString();
            Масса = pupil.Weight.ToString();
            Pupil = pupil;
        }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Пол { get; set; }
        public string Дата_рождения { get; set; }
        public string Рост { get; set; }
        public string Масса { get; set; }
        public Pupil Pupil; //ученик, который отображается
    }
}


