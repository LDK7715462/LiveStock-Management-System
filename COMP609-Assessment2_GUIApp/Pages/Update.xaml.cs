using COMP609_Assessment2_GUIApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    public partial class UpdatePage : Page
    {
        LMSApp app;
        internal UpdatePage(LMSApp app)
        {
            this.app = app;
            InitializeComponent();
            
        }


        private void InsertSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve user input from the form
                string type = SearchOptions.Text;
                string color = colours.Text;
                int weight = int.Parse(Weight.Text);
                int cost = int.Parse(Cost.Text);
                double water = double.Parse(Water.Text);
                double woolMilk = double.Parse(wool.Text);

                Animals newAnimal;

                switch (type.ToLower())
                {
                    case "cow":
                        newAnimal = new Cow("Cow", GetNewID(), water, cost, weight, color, woolMilk);
                        break;
                    case "goat":
                        newAnimal = new Goat("Goat", GetNewID(), water, cost, weight, color, woolMilk);
                        break;
                    case "sheep":
                        newAnimal = new Sheep("Sheep", GetNewID(), water, cost, weight, color, woolMilk);
                        break;
                    default:
                        MessageBox.Show("Invalid animal type.");
                        return;
       
                }

                // Insert the new animal into the database
                app.InsertAnimal(newAnimal);

                // Refresh the data displayed in the list
                AnimalList.ItemsSource = app.Animal;
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., display an error message
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private int GetNewID()
        {
            // Find the maximum ID from existing animal records and increment it
            int maxID = app.Animal.Max(a => a.ID);
            return maxID + 1;
        }


    }
}
