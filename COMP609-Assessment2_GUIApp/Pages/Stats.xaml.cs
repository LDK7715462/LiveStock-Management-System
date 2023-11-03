using COMP609_Assessment2_GUIApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Odbc;
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

namespace COMP609_Assessment2_GUIApp.Pages
{
    /// <summary>
    /// Interaction logic for Stats.xaml
    /// </summary>
    public partial class StatsPage : Page
    {
        LMSApp app;

        ObservableCollection<Animals> Animal { get; set; }

        OdbcConnection Conn;

        internal static OdbcConnection GetConn()
        {
            string? dbstr = ConfigurationManager.AppSettings.Get("odbcString");
            string fpath = @"..\..\FarmData.accdb";
            string connstr = dbstr + fpath;
            var conn = new OdbcConnection(connstr);
            conn.Open();
            return conn;
        }

        internal StatsPage(LMSApp app)
        {
            this.app = app;
            InitializeComponent();
            Conn = GetConn();
        }


        public void OkStatsBtn(object sender, RoutedEventArgs e)
        {
            if (IDStats.IsChecked == true)
            {
                CalculateButton_Click(sender, e);
                TextBlockId.Visibility = Visibility.Visible;
                TextBlockGlobal.Visibility = Visibility.Collapsed;
                StackPrice.Visibility = Visibility.Collapsed;
            }
            else if (GlobalStats.IsChecked == true)
            {

                TextBlockId.Visibility = Visibility.Collapsed;
                TextBlockGlobal.Visibility = Visibility.Visible;
                StackPrice.Visibility = Visibility.Collapsed;
            }
            else if (PriceStats.IsChecked == true)
            {
                LoadCommodityPrices();
                TextBlockId.Visibility = Visibility.Collapsed;
                TextBlockGlobal.Visibility = Visibility.Collapsed;
                StackPrice.Visibility = Visibility.Visible;
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double goatMilkPrice = 0.0, cowMilkPrice = 0.0, sheepWoolPrice = 0.0, waterPrice = 0.0, liveStockWeightTax = 0.0;

            if (int.TryParse(AnimalIDTextBox.Text, out int id))
            {
                var animal = Animal.FirstOrDefault(a => a is Animals && ((Animals)a).ID == id);

                if (animal != null)
                {
                    double income, cost, profit;

                    if (animal.GetType().Name == "Cow")
                    {
                        double cowIncome = animal.Wool_Milk * cowMilkPrice;
                        double cowCost = animal.Cost + (animal.Water * waterPrice) + (liveStockWeightTax * animal.Weight);
                        double cowProfit = cowIncome - cowCost;

                        income = cowIncome;
                        cost = cowCost;
                        profit = cowProfit;
                    }
                    else if (animal.GetType().Name == "Goat")
                    {
                        double goatIncome = animal.Wool_Milk * goatMilkPrice;
                        double goatCost = animal.Cost + (animal.Water * waterPrice) + (liveStockWeightTax * animal.Weight);
                        double goatProfit = goatIncome - goatCost;

                        income = goatIncome;
                        cost = goatCost;
                        profit = goatProfit;
                    }
                    else
                    {
                        double sheepIncome = animal.Wool_Milk * sheepWoolPrice;
                        double sheepCost = animal.Cost + (animal.Water * waterPrice) + (liveStockWeightTax * animal.Weight);
                        double sheepProfit = sheepIncome - sheepCost;

                        income = sheepIncome;
                        cost = sheepCost;
                        profit = sheepProfit;
                    }

                    ResultTextBlock.Text = string.Format(ResultTextBlock.Text, income, Environment.NewLine, cost, profit);
                    ResultTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show($"ID No [{id}] not found in the database. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid ID.");
            }
        }

        private void LoadCommodityPrices()
        {
            using (var cmd = Conn.CreateCommand())
            {
                cmd.Connection = Conn;
                string sql;
                OdbcDataReader reader;
                sql = "SELECT * FROM Commodity";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string itemName = reader["Item"].ToString();
                    double itemPrice = Convert.ToDouble(reader["Price"]);

                    switch (itemName)
                    {
                        case "CowMilk":
                            CowMilkTextBlock.Text = $"Cows Milk price (KG):                       ${itemPrice:F2}";
                            break;
                        case "GoatMilk":
                            GoatMilkTextBlock.Text = $"Goat Milk price (KG):                        ${itemPrice:F2}";
                            break;
                        case "SheepWool":
                            SheepWoolTextBlock.Text = $"Sheeps wool price (KG):                   ${itemPrice:F2}";
                            break;
                        case "Water":
                            WaterTextBlock.Text = $"Cost of water (KG):                           ${itemPrice:F2}";
                            break;
                        case "LivestockWeightTax":
                            TaxTextBlock.Text = $"Govt tax (KG per animal per day):    ${itemPrice:F2}";
                            break;
                    }
                }
                reader.Close();
            }
        }


    }
}
