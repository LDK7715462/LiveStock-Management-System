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
                DailyIDStats(sender, e);
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

        private void DailyIDStats(object sender, RoutedEventArgs e)
        {
            string enteredID = AnimalIDTextBox.Text;

            if (string.IsNullOrEmpty(enteredID))
            {
                AnimalIDTextBox.Text = "Please enter an animal ID.";
                return;
            }

            using (var cmd = Conn.CreateCommand())
            {
                string sql;
                OdbcDataReader reader;

                string animalType = "";
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

                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    var animal = Animal.FirstOrDefault(a => a is Animals && ((Animals)a).ID == id);
                    if (Animal.GetType().Name == "Cow")
                    {
                        cmd.Connection = Conn;
                        sql = $"SELECT * FROM Cow WHERE ID = '{enteredID}'";
                        cmd.CommandText = sql;
                        reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            animalType = reader["Type"].ToString();
                            water = Convert.ToDouble(reader["Water"]);
                            cost = Convert.ToDouble(reader["Cost"]);
                            weight = Convert.ToDouble(reader["Weight"]);
                            color = reader["Color"].ToString();
                            milkOrWool = Convert.ToDouble(reader["MilkOrWool"]);
                        }
                        reader.Close();
                    }
                    else if (Animal.GetType().Name == "Goat")
                    {
                        cmd.Connection = Conn;
                        sql = $"SELECT * FROM Goat WHERE ID = '{enteredID}'";
                        cmd.CommandText = sql;
                        reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            animalType = reader["Type"].ToString();
                            water = Convert.ToDouble(reader["Water"]);
                            cost = Convert.ToDouble(reader["Cost"]);
                            weight = Convert.ToDouble(reader["Weight"]);
                            color = reader["Color"].ToString();
                            milkOrWool = Convert.ToDouble(reader["MilkOrWool"]);
                        }
                        reader.Close();
                    }
                    else
                    {
                        cmd.Connection = Conn;
                        sql = $"SELECT * FROM Animal WHERE ID = '{enteredID}'";
                        cmd.CommandText = sql;
                        reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            animalType = reader["Type"].ToString();
                            water = Convert.ToDouble(reader["Water"]);
                            cost = Convert.ToDouble(reader["Cost"]);
                            weight = Convert.ToDouble(reader["Weight"]);
                            color = reader["Color"].ToString();
                            milkOrWool = Convert.ToDouble(reader["MilkOrWool"]);
                        }
                        reader.Close();
                    }
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

                double incomePerDay = (milkOrWool * (animalType == "Cow" ? cowMilkPrice : animalType == "Goat" ? goatMilkPrice : sheepWoolPrice));
                double costsPerDay = cost + (weight * waterPrice) + (weight * taxPrice);

                double totalProfitLoss = incomePerDay - costsPerDay;

                ResultTextBlock.Text = $"Animal Type: {animalType}\nWater: {water}\nCost: {cost}\nWeight: {weight}\nColor: {color}\nMilk/Wool: {milkOrWool}\n" +
                    $"Income per Day: {incomePerDay:F2}\nCosts per Day: {costsPerDay:F2}\nTotal Profit/Loss per Day: {totalProfitLoss:F2}";
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
