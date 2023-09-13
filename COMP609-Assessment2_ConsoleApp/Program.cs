using System.Configuration;
using System.Data.Odbc;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using static COMP609_Assessment2_ConsoleApp.Program.App;
using static System.Console;

namespace COMP609_Assessment2_ConsoleApp;

    class Program
    {
    static void Main(string[] args)
    {
        MainMenu();
    }

    static void MainMenu() // Can be changed, just basic structure.
    {
        var app = new App();
        while (true)
        {
            Console.WriteLine("Livestock Management System (LMS)");
            Console.WriteLine("1. Read data from database");
            Console.WriteLine("2. Display data");
            Console.WriteLine("3. Display statistics");
            Console.WriteLine("4. Query by ID/colour/livestock type/weight");
            Console.WriteLine("5. Delete record from database");
            Console.WriteLine("6. Insert record in database");
            Console.WriteLine("7. Exit");

            Console.Write("Enter your choice: ");
            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        app.PrintConsole();
                        break;
                    case 2:
                        // Implement code to display statistics.
                        break;
                    case 3:
                        // Implement code to query by ID/colour/livestock type/weight.
                        break;
                    case 4:
                        // Implement code to delete a record from the database.
                        break;
                    case 5:
                        // Implement code to insert a record into the database.
                        break;
                    case 6:
                        Console.WriteLine("Exiting the application.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }

            Console.WriteLine();
        }
    }

    internal class App
    {
        public List<LiveStockManagement> LMS { get; set; }
        OdbcConnection Conn;

        public App()
        {
            LMS = new List<LiveStockManagement>();
            this.Conn = Util.GetConn();
            ReadDB();
        }

        #region ----------------------------------------------------------------------------------- [ PRINT METHOD ] ---------------------------------------
        public void PrintConsole()
        {
            WriteLine("======= [ Live Stock Management System ] =======\n");
            string commodity;
            foreach (var commodityItem in LMS)
            {
                commodity = commodityItem.ToString();
                WriteLine(commodity);
            }
        }
        #endregion

        #region ----------------------------------------------------------------------------------- [ LIVE STOCK MANAGEMENT - METHODS ] ---------------------
        internal abstract class LiveStockManagement
        {
            public string Item { get; set; }

            public LiveStockManagement(string item)
            {
                this.Item = item;
            }
        }

        internal class Commodity : LiveStockManagement
        {
            public double Price { get; set; }
            public Commodity(string item, double price) : base(item)
            {
                this.Price = price;
            }
            public override string ToString()
            {
                return string.Format(
                    "{0}: {1,-20} {2,10:C}", // Adjust the widths as needed
                    this.GetType().Name,
                    this.Item,
                    this.Price
                );
            }
        }
        #endregion

            #region ----------------------------------------------------------------------------------- [ DATABASE METHODS ] -----------------------------------

            internal void ReadDB() // Read the database
        {
            using (var cmd = Conn.CreateCommand())
            {
                cmd.Connection = Conn;
                string sql;
                OdbcDataReader reader;

                #region ------------------------------------------------------------------ [   COMMODITY TABLE   ] -------------------------------------------
                sql = "SELECT * FROM Commodity";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string item = Util.GetString(reader["Item"]);
                    double price = Util.GetDouble(reader["Price"]);
                    if (item == Util.BAD_STRING ||
                        price == Util.BAD_DOUBLE)
                    {
                        WriteLine("Bad Row Detected");
                        continue; // Corrupted Row, Skips
                    }
                    var commodity = new Commodity(item, price);
                    LMS.Add(commodity);
                }
                reader.Close();
                #endregion

                #region ------------------------------------------------------------------ [   COW TABLE   ] ---------------------------------------------

                #endregion

                #region ------------------------------------------------------------------ [   GOAT TABLE   ] ---------------------------------------------

                #endregion

                #region ------------------------------------------------------------------ [   SHEEP TABLE   ] ---------------------------------------------

                #endregion
            }
        }

        internal static class Util
        {
            internal static readonly string BAD_STRING = string.Empty;
            internal static readonly int BAD_INT = Int32.MinValue;
            internal static readonly double BAD_DOUBLE = Double.MinValue;
            internal static int GetInt(object o)
            {
                if (o == null) return BAD_INT;
                int n;
                if (int.TryParse(o.ToString(), out n) == false)
                    return BAD_INT;
                return n;
            }
            internal static double GetDouble(object o)
            {
                if (o == null) return BAD_DOUBLE;
                double n;
                if (double.TryParse(o.ToString(), out n) == false)
                    return BAD_DOUBLE;
                return n;
            }

            internal static string GetString(object o)
            {
                return o?.ToString() ?? BAD_STRING;
            }

            internal static OdbcConnection GetConn()
            {
                string? dbstr = ConfigurationManager.AppSettings.Get("odbcString");
                string fpath = @"..\..\FarmData.accdb";
                string connstr = dbstr + fpath;
                var conn = new OdbcConnection(connstr);
                conn.Open();
                return conn;
            }
            #endregion
        }
    }
}