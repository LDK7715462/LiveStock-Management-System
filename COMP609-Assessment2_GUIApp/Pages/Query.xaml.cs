using COMP609_Assessment2_GUIApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            string selectedFilter = (string)((ComboBoxItem)SearchOptions.SelectedItem).Content;
            string searchText = Search.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                // Handle empty search text (show all data or display a message)
                AnimalList.ItemsSource = app.Animal;
                return;
            }

            List<Animals> filteredAnimals = new List<Animals>();




            switch (selectedFilter)
            {
                case "ID":
                    if (int.TryParse(searchText, out int searchID))
                    {
                        filteredAnimals = app.Animal.Where(a => a.ID == searchID).ToList();
                    }
                    else
                    {
                        // Handle the case where the input is not a valid number
                        MessageBox.Show("No Stock Found");
                        // You can also clear the results if needed:
                        // AnimalList.ItemsSource = null; // Clear the results
                    }
                    break;
                case "Colour":
                    filteredAnimals = app.Animal.Where(a => a.Colour.ToLower().Contains(searchText.ToLower())).ToList();
                    break;
                case "Type":
                    filteredAnimals = app.Animal.Where(a => a.Type.ToLower().Contains(searchText.ToLower())).ToList();
                    break;
                case "Weight":
                    if (double.TryParse(searchText, out double searchWeight))
                    {
                        filteredAnimals = app.Animal.Where(a => a.Weight.ToString().Contains(searchText)).ToList();
                    }
                    else
                    {
                        // Handle the case where the input is not a valid number
                        // You can show a message to the user or clear the results
                        AnimalList.ItemsSource = null; // Clear the results
                    }
                    break;
                case "Cost":
                    if (double.TryParse(searchText, out double searchCost))
                    {
                        filteredAnimals = app.Animal.Where(a => a.Cost.ToString().Contains(searchText)).ToList();
                    }
                    else
                    {
                        // Handle the case where the input is not a valid number
                        // You can show a message to the user or clear the results
                        AnimalList.ItemsSource = null; // Clear the results
                    }
                    break;
                case "Water":
                    if (double.TryParse(searchText, out double searchWater))
                    {
                        filteredAnimals = app.Animal.Where(a => a.Water.ToString().Contains(searchText)).ToList();
                    }
                    else
                    {
                        // Handle the case where the input is not a valid number
                        // You can show a message to the user or clear the results
                        AnimalList.ItemsSource = null; // Clear the results
                    }
                    break;
                case "Milk Volume":
                    if (double.TryParse(searchText, out double searchMilk))
                    {
                        filteredAnimals = app.Animal.Where(a => a.Wool_Milk.ToString().Contains(searchText)).ToList();
                    }
                    else
                    {
                        // Handle the case where the input is not a valid number
                        // You can show a message to the user or clear the results
                        AnimalList.ItemsSource = null; // Clear the results
                    }
                    break;
                    // Add more cases for other filter criteria as needed
            }


            if (filteredAnimals.Count == 0)
            {
                NoResultsMessage.Visibility = Visibility.Visible; // Show the message
            }
            else
            {
                NoResultsMessage.Visibility = Visibility.Collapsed; // Hide the message
            }

            AnimalList.ItemsSource = filteredAnimals;
        }




        private void AnimalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AnimalList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}