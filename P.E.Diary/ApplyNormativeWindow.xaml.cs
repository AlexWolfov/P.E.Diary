﻿using System;
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
using System.Windows.Shapes;

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
