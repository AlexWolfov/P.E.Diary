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

        private string GetSelectedNodeText()
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
                        item.ItemsSource = normativesNamesByTypes[item.Header.ToString()];
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
        /*private void NormativesList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (MainList.SelectedItem != null)
            {
                if (((TreeViewItem)MainList.SelectedItem).Parent == null) //если родителя нет, то это категория
                {
                    EditType editType = new EditType(MainList.SelectedValue.ToString(), this);
                    editType.Show();
                }
                else //если родитель есть, то это норматив
                {
                    EditNormative editNormativeForm = new EditNormative(
                        Normatives[MainList.SelectedValue.ToString()], this);
                    editNormativeForm.Show();
                }
            }
        }*/
        #endregion
    }
}
