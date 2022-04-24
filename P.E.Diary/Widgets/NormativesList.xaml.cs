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
    /// Interaction logic for NormativesList.xaml
    /// </summary>
    public partial class NormativesList : UserControl
    {
        private static List<String> Types;
        private static Dictionary<string, Normative> Normatives;
        private static List<NormativesList> _normativesLists; //список со весми списками нормативов. Нужен для параллельного их редактирования

        public NormativesList()
        {
            if (_normativesLists == null)
            {
                _normativesLists = new List<NormativesList>();
            }
            _normativesLists.Add(this);
            InitializeComponent();
            LoadNormatives();
        }

        #region Back

        private void DeleteSelectedType()
        {
            SqlReader.DeleteType(GetSelectedNodeText());
            LoadNormatives();
        }

        private void DeleteSelectedNormative()
        {
            SqlReader.DeleteNormative(GetActiveNormative());
            LoadNormatives();
        }

        public string GetSelectedNodeText()
        {
            return ((TreeViewItem)MainList.SelectedItem).Header.ToString();
        }

        public Normative GetActiveNormative()
        {
            return Normatives[GetSelectedNodeText()];
        }

        public void CreateNormative()
        {
            NewNormativeDialog createNormativeForm = new NewNormativeDialog(this);
            createNormativeForm.Show();
        }

        public void DeleteNode()
        {
            if (MainList.SelectedItem != null)
            {
                if (FoolProof.DeletionProtection(GetSelectedNodeText()))
                {
                    if (((TreeViewItem) MainList.SelectedItem).Parent != null) //если родителю есть, то это категория
                    {
                        DeleteSelectedType();
                    }
                    else //если родителя нет, то это норматив
                    {
                        DeleteSelectedNormative();
                    }
                }
            }
            else
            {
                FoolProof.UniversalProtection("Выберите норматив или категорию");
            }
        }

        private void LoadData() //для кадого списка нормативов выводим актуальные данные
        {
            foreach (NormativesList normativesList in _normativesLists)
            {
                normativesList.MainList.Items.Clear();
                foreach (string type in Types)
                {
                    TreeViewItem newItem = new TreeViewItem();
                    newItem.Header = type;
                    normativesList.MainList.Items.Add(newItem);
                }
                Dictionary<string, List<TreeViewItem>> normativesNamesByTypes = new Dictionary<string, List<TreeViewItem>>(); //словарь нодов с именами нормативов по катериям
                foreach (Normative normative in Normatives.Values)
                {
                   try
                    {
                        normativesNamesByTypes[normative.Type].Add(new TreeViewItem { Header = normative.Name });
                    }
                    catch(KeyNotFoundException ex) //если еще нет ключа
                    {
                        normativesNamesByTypes.Add(normative.Type, new List<TreeViewItem> { new TreeViewItem { Header = normative.Name} });
                    }
                }
                foreach (TreeViewItem item in normativesList.MainList.Items)
                {
                    try
                    {
                        item.MouseDoubleClick += TypeItem_Edit;
                        item.ItemsSource = normativesNamesByTypes[item.Header.ToString()];
                        foreach (TreeViewItem normativeItem in item.Items)
                        {
                            normativeItem.MouseDoubleClick += NormativeItem_Edit;
                        }
                    }
                    catch (KeyNotFoundException ex)
                    { }
                }
            }
        }

        public void LoadNormatives() //загружаем актуальные данные
        {
            Types = SqlReader.LoadTypes();
            Normatives = SqlReader.LoadNormatives();
            LoadData();
        }
        #endregion

        #region Events
        
        private void TypeItem_Edit(object sender, MouseButtonEventArgs e)
        {
            EditTypeDialog editTypeDialog = new EditTypeDialog(this, GetSelectedNodeText());
        }

        private void NormativeItem_Edit(object sender, MouseButtonEventArgs e)
        {
            EditNormativeDialog editNormativeDialog = new EditNormativeDialog(this, GetActiveNormative());
        }

        private void ContextMenuAddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NewNormativeDialog newNormativeDialog = new NewNormativeDialog(this);
        }
        
        private void ContextMenuEditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MainList.SelectedItem != null)
            {
                if (((TreeViewItem)MainList.SelectedItem).Parent != null) //если родителю есть, то это категория
                {
                    EditTypeDialog editTypeDialog = new EditTypeDialog(this, GetSelectedNodeText());
                }
                else //если родителя нет, то это норматив
                {
                    EditNormativeDialog editNormativeDialog = new EditNormativeDialog(this, GetActiveNormative());
                }
            }
        } 
        
        private void ContextMenuDeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DeleteNode();
        }

        private void ContextMenuReloadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadNormatives();
        }

        private void ContextMenuDeleteAddButtons_Opened(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MainList.SelectedItem == null)
            {
                foreach (MenuItem item in ContextMenuDeleteAddButtons.Items)
                {
                    if (item.Name == "DeleteButton")
                    {
                        item.IsEnabled = false;
                    }
                    if (item.Name == "EditButton")
                    {
                        item.IsEnabled = false;
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
                    }
                    if (item.Name == "EditButton")
                    {
                        item.IsEnabled = true;
                    }
                }
            }
        }


        #endregion
    }
}
