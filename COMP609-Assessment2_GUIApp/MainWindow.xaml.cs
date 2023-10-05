using COMP609_Assessment2_GUIApp.Views;
using COMP609_Assessment2_GUIApp.Models;
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

namespace COMP609_Assessment2_GUIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LMSApp app;

        public MainWindow()
        {
            InitializeComponent();
            app = new LMSApp();
        }

        private void TabControl_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = TabMenu.SelectedIndex;
            MainView.Content = index switch
            {
                0 => new TestPage(app),
                1 => new TestPage(app),
                2 => new TestPage(app),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
