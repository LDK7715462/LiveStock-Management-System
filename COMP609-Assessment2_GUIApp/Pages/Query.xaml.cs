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

namespace COMP609_Assessment2_GUIApp.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class QueryPage : Page
    {
        LMSApp app;
        internal QueryPage(LMSApp app)
        {
            this.app = app;
            InitializeComponent();
            AnimalList.ItemsSource = app.Animal;
        }

        private void Auto_Search(object sender, TextChangedEventArgs e)
        {
            string searchText = Search.Text;
            ComboBoxItem selectedSortItem = (ComboBoxItem)SearchOptions.SelectedItem;

            if (string.IsNullOrEmpty(searchText))
            {
                AnimalList.ItemsSource = app.Animal;
                return;
            }

            var filteredAnimals = app.Animal.Where(a => a.Type.ToLower().Contains(searchText.ToLower())).ToList();

            string? sortBy = selectedSortItem?.Content?.ToString(); // Safe access using ?. operator


            if (sortBy == "ID")
            {

                filteredAnimals = filteredAnimals.OrderBy(a => a.ID).ToList();

            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "Colour":
                        filteredAnimals = filteredAnimals.OrderBy(a => a.Colour).ToList();
                        break;
                    case "Type":
                        filteredAnimals = filteredAnimals.OrderBy(a => a.Type).ToList();
                        break;
                        // Add more cases for sorting by other properties if needed
                }
            }

            AnimalList.ItemsSource = filteredAnimals;
        }



        private void AnimalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
