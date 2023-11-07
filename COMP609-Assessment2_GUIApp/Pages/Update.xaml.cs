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












        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Retrieve the ID to delete from the TextBox
                if (int.TryParse(DeleteID.Text, out int deleteID))
                {
                    string tbl = app.Animal.FirstOrDefault(a => a.ID == deleteID)?.GetType().Name;
                    if (tbl != null)
                    {
                        if (app.DeleteByID(tbl, deleteID))
                        {
                            // Successfully deleted the record
                            // Refresh the data displayed in the list

                            DeleteResult.Text = "Record deleted successfully.";
                        }
                        else
                        {
                            DeleteResult.Text = "Error in deleting the record.";
                        }
                    }
                    else
                    {
                        DeleteResult.Text = "No record found with the given ID.";
                    }
                }
                else
                {
                    DeleteResult.Text = "Invalid ID input. Please enter a valid ID.";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., display an error message
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        public bool DeleteByID(string tbl, int id)
        {
            try
            {
                var animal = app.Animal.FirstOrDefault(a => a.ID == id);
                if (animal != null)
                {
                    // Successfully deletes from the database
                    if (app.Animal.Remove(animal))
                    {
                        return true;
                    }
                    else
                    {
                        return false; // Remove from the collection failed
                    }
                }
                return false; // Record not found in the collection
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., log or display an error message
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }



        // Delete Animal by ID




    }
}
