﻿using COMP609_Assessment2_GUIApp.Models;
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

namespace COMP609_Assessment2_GUIApp.Views
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : UserControl
    {
        LMSApp app;
        internal TestPage(LMSApp app)
        {
            this.app = app;
            InitializeComponent();
            AnimalList.ItemsSource = app.Animal;
        }
    }
}
