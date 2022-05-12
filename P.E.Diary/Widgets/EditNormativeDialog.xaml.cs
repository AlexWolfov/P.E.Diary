using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for EditNormativeDialog.xaml
    /// </summary>
    public partial class EditNormativeDialog : Window
    {

        private NormativesList _normativesList;
        private Normative _editingNormative;

        public EditNormativeDialog(NormativesList normativesList, Normative normative)
        {
            _normativesList = normativesList;
            _editingNormative = normative;
            InitializeComponent();
            LoadTypes();
            InsertData();
            Show();
        }

        private void InsertData()
        {
            NameBox.Text = _editingNormative.Name;
            Formula.Text = _editingNormative.Formula;
            if (_editingNormative.Ranges.Count > 0)
            {
                TablePath.Text = "Таблица в базе данных";
            }
            else
            {
                TablePath.Text = "Нет таблицы";
            }
        }

        private void CreateNewNormative()
        {
            _editingNormative.Name = NameBox.Text;
            _editingNormative.Formula = Formula.Text;
            if (TypeSelection.SelectedItem != null)
            {
                _editingNormative.Type = TypeSelection.SelectedItem.ToString();
            }
            else
            {
                _editingNormative.Type = "Без категории";
            }
            SqlReader.EditNormative(_editingNormative);
            _normativesList.LoadNormatives();
        }

        public void LoadTypes()
        {
            if ((TypeSelection != null))
            {
                TypeSelection.Items.Clear();
            }
            List<string> typesList = SqlReader.LoadTypes();
            foreach (string typeName in typesList)
            {
                TypeSelection.Items.Add(typeName);
            }
        }

        private void NewType_Click(object sender, EventArgs e)
        {
            NewTypeDialog newTypeForm = new NewTypeDialog(this);
            newTypeForm.Show();
        }

        private void DeleteType_Click(object sender, EventArgs e)
        {
            if (TypeSelection.SelectedItem != null)
            {
                if (FoolProof.DeletionProtection(TypeSelection.SelectedItem.ToString())) ;
                {
                    SqlReader.DeleteType(TypeSelection.SelectedItem.ToString());
                    _normativesList.LoadNormatives();
                    LoadTypes();
                }
            }
            else
            {
                FoolProof.UniversalProtection("Нельзя удалить отсутствующую категорию");
            }
        }

        private void EditNormative_Click(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text != "" && Formula.Text != "")
            {
                CreateNewNormative();
                Close();
            }
            else
            {
                FoolProof.UniversalProtection("Заполните имя и формулу");
            }
        }

        private void SaveTable_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.AddExtension = true;
            saveFileDialog.ShowDialog();
            string fileName = saveFileDialog.FileName;
            if (fileName != "" && _editingNormative.Ranges != null)
            {
                StreamWriter streamWriter = new StreamWriter(fileName, true, System.Text.Encoding.UTF8);
                string line = "Оценка;";
                for (int i = 0; i < _editingNormative.Ranges.Count-1; i++)
                {
                    line += (i + Normative.minAge) + " лет" + ";";
                }
                line += (Normative.minAge + _editingNormative.Ranges.Count - 1);
                streamWriter.WriteLine(line);
                foreach (string key in _editingNormative.Ranges[0].Keys)
                {
                    line = key+";";
                    for (int i = 0; i < _editingNormative.Ranges[0].Keys.Count - 1; i++)
                    {
                        line += _editingNormative.Ranges[i][key] + ";";
                    }
                    line += _editingNormative.Ranges[_editingNormative.Ranges[0].Keys.Count - 1][key];
                    streamWriter.WriteLine(line);
                }
                streamWriter.Close();
            }
            else
            {
                FoolProof.UniversalProtection("Выберите корректный путь");
            }
        }

        private void LoadTable_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string tableFileName = openFileDialog.FileName;
            TablePath.Text = openFileDialog.FileName;
            if (tableFileName != "")
            {
                try
                {
                    _editingNormative.InsertRanges(tableFileName);
                    SqlReader.EditNormativeRanges(_editingNormative);
                }
                catch (Exception ex)
                {
                    FoolProof.UniversalProtection("Неправильный файл");
                }
            }
            else
            {
                FoolProof.UniversalProtection("Вы не выбрали файл");
            }
        }
    }
}
