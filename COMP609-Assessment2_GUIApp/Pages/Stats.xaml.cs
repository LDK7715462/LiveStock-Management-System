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
            Conn = Util.GetConn();
        }


        public void OkStatsBtn(object sender, RoutedEventArgs e)
        {
            if (IDStats.IsChecked == true)
            {
                DailyIDStats(sender, e);
                TextBlockId.Visibility = Visibility.Visible;
                TextBlockGlobal.Visibility = Visibility.Collapsed;
                StackPrice.Visibility = Visibility.Collapsed;
            }
            else if (GlobalStats.IsChecked == true)
            {
                GetGlobalStats();
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

        #region DailyIDStats
        private void DailyIDStats(object sender, RoutedEventArgs e)
        {
            string enteredID = AnimalIDTextBox.Text;

            if (string.IsNullOrEmpty(enteredID))
            {
                ResultTextBlock.Text = "Please enter an animal ID.";
                return;
            }

            using (var cmd = Conn.CreateCommand())
            {
                string sql;
                OdbcDataReader reader;

                double water = 0;
                double cost = 0;
                double weight = 0;
                string color = "";
                double milkOrWool = 0;

                double cowMilkPrice = 0;
                double goatMilkPrice = 0;
                double sheepWoolPrice = 0;
                double waterPrice = 0;
                double taxPrice = 0;

                if (int.TryParse(enteredID, out int id))
                {
                    var animal = app.Animal.FirstOrDefault(a => a is Animals && ((Animals)a).ID == id);
                    if (animal != null)
                    {
                        if (animal is Cow cow)
                        {
                            water = cow.Water;
                            cost = cow.Cost;
                            weight = cow.Weight;
                            color = cow.Colour;
                            milkOrWool = cow.Wool_Milk;
                        }
                        else if (animal is Goat goat)
                        {
                            water = goat.Water;
                            cost = goat.Cost;
                            weight = goat.Weight;
                            color = goat.Colour;
                            milkOrWool = goat.Wool_Milk;
                        }
                        else if (animal is Sheep sheep)
                        {
                            water = sheep.Water;
                            cost = sheep.Cost;
                            weight = sheep.Weight;
                            color = sheep.Colour;
                            milkOrWool = sheep.Wool_Milk;

                        }
                    }
                    else
                    {
                        ResultTextBlock.Text = "Animal ID does not exist in the system. Please enter a valid ID.";
                        return;
                    }


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
                                cowMilkPrice = itemPrice;
                                break;
                            case "GoatMilk":
                                goatMilkPrice = itemPrice;
                                break;
                            case "SheepWool":
                                sheepWoolPrice = itemPrice;
                                break;
                            case "Water":
                                waterPrice = itemPrice;
                                break;
                            case "LivestockWeightTax":
                                taxPrice = itemPrice;
                                break;
                        }
                    }
                    reader.Close();

                    double incomePerDay = (milkOrWool * milkOrWool);
                    double costsPerDay = cost + (weight * waterPrice) + (weight * taxPrice);

                    double totalProfitLoss = incomePerDay - costsPerDay;

                    ResultTextBlock.Text =
                        $"Animal Type: \t\t\t{animal.Type:F2}\n" +
                        $"Water: \t\t\t\t{water:F2} Litres\n" +
                        $"Operating Cost: \t\t\t${cost:F2}\n" +
                        $"Current Weight: \t\t\t{weight:F2} KG\n" +
                        $"Colour: \t\t\t\t{color}\n" +
                        $"Milk Production: \t\t\t{milkOrWool:F2} KG\n" +
                        "--------------------------------------------------------------------------------\n" +
                        $"Total Daily Income: \t\t${incomePerDay:F2} \n" +
                        $"Total expenditure per day: \t\t${costsPerDay:F2}\n" +
                        $"Total profit/ loss: \t\t\t${totalProfitLoss:F2} \n";
                }

            }
        }
        #endregion

        #region GlobalStats
        private void GetGlobalStats()
        {

            double goatMilkPrice = 0.0, cowMilkPrice = 0.0, sheepWoolPrice = 0.0, waterPrice = 0.0, liveStockWeightTax = 0.0;
            double cowMilk = 0, cowCost = 0, cowWater = 0;
            double sheepWool = 0, sheepCost = 0, sheepWater = 0;
            double goatMilk = 0, goatCost = 0, goatWater = 0;
            double totalTax = 0, totalWeight = 0;
            int animalCount = 0;

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
                            cowMilkPrice = itemPrice;
                            break;
                        case "GoatMilk":
                            goatMilkPrice = itemPrice;
                            break;
                        case "SheepWool":
                            sheepWoolPrice = itemPrice;
                            break;
                        case "Water":
                            waterPrice = itemPrice;
                            break;
                        case "LivestockWeightTax":
                            liveStockWeightTax = itemPrice;
                            break;
                    }
                }
                reader.Close();

                cmd.CommandText = "SELECT * FROM cow";
                using (var cowReader = cmd.ExecuteReader())
                {
                    while (cowReader.Read())
                    {//takes each item in milk, cost, water and adds them together into their 'total' variable
                        double milk = cowReader.GetDouble(cowReader.GetOrdinal("Milk"));
                        double cost = cowReader.GetDouble(cowReader.GetOrdinal("Cost"));
                        double water = cowReader.GetDouble(cowReader.GetOrdinal("Water"));
                        cowMilk += milk; cowCost += cost; cowWater += water;

                        double cowWeight = cowReader.GetDouble(cowReader.GetOrdinal("Weight"));
                        totalWeight += cowWeight;
                        animalCount++;
                    }
                }
                cmd.CommandText = "SELECT * FROM sheep";
                using (var sheepReader = cmd.ExecuteReader())
                {
                    while (sheepReader.Read())
                    {//takes each item in milk, cost, water and adds them together into their 'total' variable
                        double wool = sheepReader.GetDouble(sheepReader.GetOrdinal("Wool"));
                        double cost = sheepReader.GetDouble(sheepReader.GetOrdinal("Cost"));
                        double water = sheepReader.GetDouble(sheepReader.GetOrdinal("Water"));
                        sheepWool += wool; sheepCost += cost; sheepWater += water;

                        double sheepWeight = sheepReader.GetDouble(sheepReader.GetOrdinal("Weight"));
                        totalWeight += sheepWeight;
                        animalCount++;
                    }
                }
                cmd.CommandText = "SELECT * FROM goat";
                using (var goatReader = cmd.ExecuteReader())
                {
                    while (goatReader.Read())
                    {//takes each item in milk, cost, water and adds them together into their 'total' variable
                        double milk = goatReader.GetDouble(goatReader.GetOrdinal("Milk"));
                        double cost = goatReader.GetDouble(goatReader.GetOrdinal("Cost"));
                        double water = goatReader.GetDouble(goatReader.GetOrdinal("Water"));
                        goatMilk += milk; goatCost += cost; goatWater += water;

                        double goatWeight = goatReader.GetDouble(goatReader.GetOrdinal("Weight"));
                        totalWeight += goatWeight;
                        animalCount++;
                    }
                }
                totalTax = totalWeight * liveStockWeightTax * 30;
                double avgWeight = totalWeight / animalCount;
                double totalIncome = (cowMilk * cowMilkPrice) + (sheepWool * sheepWoolPrice) + (goatMilk * goatMilkPrice);
                double totalCost = (cowCost + sheepCost + goatCost) + (cowWater * waterPrice) + (sheepWater * waterPrice) + (goatWater * waterPrice) + totalTax;
                double totalProfit = totalIncome - totalCost;

                GCowMilkTextBlock.Text = $"Total Yield of cows milk per day: \t\t\t\t{cowMilk:N1} KG";
                GCowCostTextBlock.Text = $"Total operation cost of all cows per day: \t\t\t{cowCost:C}";
                GCowWaterTextBlock.Text = $"Total water consumed by all cows: \t\t\t\t{cowWater:F2} Litres";

                GSheepWoolTextBlock.Text = $"Total yield of wool per day: \t\t\t\t{sheepWool:N1} KG";
                GSheepCostTextBlock.Text = $"Total operation cost of all sheep per day: \t\t\t{sheepCost:C}";
                GSheepWaterTextBlock.Text = $"Total water consumed by all sheep: \t\t\t\t{sheepWater:F2} Litres";

                GGoatMilkTextBlock.Text = $"Total Yield of goats milk per day: \t\t\t\t{goatMilk:N1} KG";
                GGoatCostTextBlock.Text = $"Total operation cost of all goats per day: \t\t\t{goatCost:C}";
                GGoatWaterTextBlock.Text = $"Total water consumed by all goats: \t\t\t\t{goatWater:F2} Litres\n" +
                    "--------------------------------------------------------------------------------";
                GTotalTaxTextBlock.Text = $"Total tax for all animals per 30-day cycle: \t\t\t{totalTax:C}";
                GTotalWeightTextBlock.Text = $"Total weight of all animals currently in system: \t\t{totalWeight:N1} KG";
                GAnimalCountTextBlock.Text = $"Total Animals currently in the system: \t\t\t{animalCount.ToString()}";
                GTotalAvgWeightTextBlock.Text = $"Average weight of all animals currently in system: \t\t{avgWeight:N1} KG";
                GTotalIncomeTextBlock.Text = $"Total income from all animals in the database: \t\t{totalIncome:C}";
                GTotalCostTextBlock.Text = $"Total costs incurred daily from all animals in the database: \t{totalCost:C}";
                GTotalProfitTextBlock.Text = $"Total profit gained from all animals in the database: \t\t{totalProfit:C}";
            }
        }

        #endregion

        #region LoadCommodityPrices
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
        #endregion

    }
}

