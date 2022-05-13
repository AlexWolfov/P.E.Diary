using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for NewNormativeDialog.xaml
    /// </summary>
    public partial class NewNormativeDialog : Window
    {
        private NormativesList _normativesList;
        private string _tableFileName;

        public NewNormativeDialog(NormativesList normativesList)
        {
            _normativesList = normativesList;
            InitializeComponent();
            LoadTypes();
            Show();
        }

        private void CreateNewNormative()
        {
            Normative normative = new Normative(
                name: NameBox.Text, formula: Formula.Text, type: "Без категории");
            if (TypeSelection.SelectedItem != null)
            {
                normative.Type = TypeSelection.SelectedItem.ToString();
            }
            else
            {
                normative.Type = "Без категории";
            }
            normative.Id = SqlReader.CreateNormative(normative); // сразу же получаем Id норматива, нужно для привязки к нему таблицы оценивания в БД
            if (_tableFileName != "")
            {
                try
                {
                    normative.InsertRanges(_tableFileName);
                }
                catch (Exception ex)
                {
                    FoolProof.UniversalProtection("Ошибка в файле");
                }
                SqlReader.AddNortmativeRanges(normative);
            }
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

        private void CreateNormative_Click(object sender, RoutedEventArgs e)
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

        private void LoadTable_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            _tableFileName = openFileDialog.FileName;
            TablePath.Text = openFileDialog.FileName;
        }
    }
}
