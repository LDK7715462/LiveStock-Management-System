using COMP609_Assessment2_GUIApp.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace COMP609_Assessment2_GUIApp.Pages
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Page
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

        internal Home(LMSApp app)
        {
            this.app = app;
            InitializeComponent();
            Conn = Util.GetConn();
            Clock();
            stats();
        }

        private void Clock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object? sender, EventArgs e)
        {
            dateTime.Text = DateTime.Now.ToString();
        }

        public void stats()
        {

            double goatMilkPrice = 0.0, cowMilkPrice = 0.0, sheepWoolPrice = 0.0, waterPrice = 0.0, liveStockWeightTax = 0.0;
            double cowMilk = 0, cowCost = 0, cowWater = 0;
            double sheepWool = 0, sheepCost = 0, sheepWater = 0;
            double goatMilk = 0, goatCost = 0, goatWater = 0;
            double totalTax = 0, totalWeight = 0;
            int animalCount = 0, cowCount = 0, sheepCount = 0, goatCount = 0;

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
                        cowCount++;
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
                        sheepCount++;
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
                        goatCount++;
                        animalCount++;
                    }
                }

                // Set the values in the TextBlocks
                cowsMilkPrice.Text = $"{cowMilkPrice:C}";
                goatsMilkPrice.Text = $"{goatMilkPrice:C}";
                sheepsWoolPrice.Text = $"{sheepWoolPrice:C}";

                // Set the values in the TextBlocks with animal type and count
                animalCount1.Text = "Cow:     " + cowCount;
                animalCount2.Text = "Sheep:  " + sheepCount;
                animalCount3.Text = "Goat:    " + goatCount;


            }
        }
    }
}
