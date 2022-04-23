using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace P.E.Diary.Widgets
{

    //// <summary>
    /// Interaction logic for PupilsTable.xaml
    /// </summary>
    public partial class PupilsTable : UserControl
    {
        public SchoolClass CurrentClass;
        private bool _cellEditIsCorrect = false;

        public PupilsTable()
        {
            InitializeComponent();
        }


        public void LoadTable(SchoolClass newClass)
        {
            List<PupilRow> pupilRows = new List<PupilRow>();
            Table.ItemsSource = pupilRows;
            if (!(newClass is null))
            {
                CurrentClass = newClass;
                foreach (var pupil in CurrentClass.Pupils)
                {
                    pupilRows.Add(new PupilRow(pupil.Surname, pupil.Name, pupil.Gender, pupil.Age.ToString(), pupil.Height.ToString(), pupil.Weight.ToString()));
                }
            }
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
                case "Возраст":
                    return selectedItem.Возраст;
                    break;
                case "Высота":
                    return selectedItem.Высота;
                    break;
                case "Вес":
                    return selectedItem.Вес;
                    break;
                default:
                    break;
            }
            return "";
        }

        private void EditCurrentCell(string newData)
        {
            EditCellInRow(GetCurrentCellAdressAxesY(), GetCurrentCellAdressAxesX(), newData);
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
                case "Возраст":
                    selectedItem.Возраст = newData;
                    break;
                case "Высота":
                    selectedItem.Высота = newData;
                    break;
                case "Вес":
                    selectedItem.Вес = newData;
                    break;
                default:
                    break;
            }
        }


        private void EditRow(string value)
        {
            if (CurrentClass != null && GetCurrentCellAdressAxesY() != -1)
            {
                Pupil pupil = CurrentClass.Pupils[GetCurrentCellAdressAxesY()]; //считывем изменяемого ученика
                switch (Table.CurrentColumn.Header)
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
                            EditCurrentCell(pupil.Gender);
                        }
                        break;
                    case "Возраст":
                        if (!_cellEditIsCorrect)
                        {
                            string date = FoolProof.ReturnDate(pupil.Birthday, value);
                            if (date != "-1")
                            {
                                pupil.Birthday = date;
                            }
                            _cellEditIsCorrect = true;
                            EditCurrentCell(pupil.Age.ToString());
                        }
                        else
                        {
                            _cellEditIsCorrect = false;
                        }
                        break;
                    case "Высота":
                        FoolProof.SetInt(ref pupil.Height, value);
                        EditCurrentCell(pupil.Height.ToString());
                        break;
                    case "Вес":
                        FoolProof.SetInt(ref pupil.Weight, value);
                        EditCurrentCell(pupil.Weight.ToString());
                        break;
                    default:
                        break;
                }

                CurrentClass.Pupils[GetCurrentCellAdressAxesY()] = pupil;
                SqlReader.EditPupil(pupil);
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

        private void DeleteRow()
        {
            Pupil pupil = CurrentClass
                .Pupils[GetCurrentCellAdressAxesY()]; //считывем удалемого ученика
            SqlReader.DeletePupil(pupil.Id); //удаляем ученика из базы Данных
            CurrentClass.Pupils.Remove(
                CurrentClass.Pupils[GetCurrentCellAdressAxesY()]); //удалием ученика из списка класса
        }

        
        private Pupil RowToPupil(int rowX)
        {
            return CurrentClass.Pupils[rowX];
        }


        public List<Pupil> ReturnSelectedPupils()
        {
            List<Pupil> result = new List<Pupil>();
            var rows = Table.SelectedItems;
            foreach (DataGridRow row in rows)
            {
                result.Add(RowToPupil(row.GetIndex()));
            }
            return result;
        }


        #region Events

        private void Table_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            CurrentClass = SqlReader.LoadClasses()["9 Е"];
            LoadTable(CurrentClass);
        }

        private void Table_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string newValue = (e.EditingElement as TextBox).Text;
            EditRow(newValue);
        }

        private void Table_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (CurrentClass != null)
            {
                if (FoolProof.DeletionProtection("ученика"))
                { 
                    DeleteRow();
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
                    DeleteRow();
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
                    DeleteRow();
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
                    DeleteRow();
                }
            }
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            List <PupilRow> rows = (List<PupilRow>) Table.ItemsSource;
            AddRow();
        }

        #endregion
    }

    public class PupilRow
    {
        public PupilRow(string surname, string name, string gender, string age, string height, string weight)
        {
            Фамилия = surname;
            Имя = name;
            Пол = gender;
            Возраст = age;
            Высота = height;
            Вес = weight;
        }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Пол { get; set; }
        public string Возраст { get; set; }
        public string Высота { get; set; }
        public string Вес { get; set; }
    }
}


