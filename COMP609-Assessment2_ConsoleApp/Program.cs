using System.Configuration;
using System.Data.Odbc;
using System.IO;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Diagnostics;

namespace COMP609_Assessment2_ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "LiveStock Management System";
            var app = new App();

            #region MAIN MENU / SWITCH
            while (true)
            {
                int opt = MainMenu();
                switch (opt)
                {
                    case 1:
                    // Display Data Menu
                        Console.Clear();
                        app.DisplayDataSwitch();
                        break;
                    case 2:
                    // Query Data Menu
                        Console.Clear();
                        app.QueryDataSwitch();
                        break;
                    // Exit Console Program
                    case 3:
                        Console.Clear();
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static int MainMenu()
        {
            int opt = 0;
            bool validInput = false;
            while (!validInput)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                string title = @"
  _     _____     _______  ____ _____ ___   ____ _  __     ______   ______ _____ _____ __  __ 
 | |   |_ _\ \   / / ____|/ ___|_   _/ _ \ / ___| |/ /    / ___\ \ / / ___|_   _| ____|  \/  |
 | |    | | \ \ / /|  _|  \___ \ | || | | | |   | ' /     \___ \\ V /\___ \ | | |  _| | |\/| |
 | |___ | |  \ V / | |___  ___) || || |_| | |___| . \      ___) || |  ___) || | | |___| |  | |
 |_____|___|  \_/  |_____||____/ |_| \___/ \____|_|\_\    |____/ |_| |____/ |_| |_____|_|  |_|
                                                              ╔═════════════════════════════╗
                                                              ║ LiveStock Managment System  ║
                                                              ║  by Dylan, Tristan & Lucky  ║
                                                              ╚═════════════════════════════╝";

                Console.WriteLine(title);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("******************* [ Main Menu ] ********************");
                Console.WriteLine("*                                                    *");
                Console.WriteLine("*              1 - Display Data Menu                 *");
                Console.WriteLine("*              2 - Query Data Menu                   *");
                Console.WriteLine("*              3 - Exit                              *");
                Console.WriteLine("*                                                    *");
                Console.WriteLine("******************************************************");
                Console.WriteLine();
                Console.WriteLine("Enter an Option: ");

                try
                {
                    opt = int.Parse(Console.ReadLine());
                    if (opt >= 1 && opt <= 3)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid choice. Please try again.\n");
                    }
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please enter a valid option (1-6).\n");
                }
            }
            return opt;
        }
        #endregion
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

        #region ----------------------------------------------------------------------------------- [ PRINT METHODS ] ---------------------------------------

        #region DISPLAY DATA MENU / SWITCH
        public void DisplayDataSwitch()
        {
            string item;
            while (true)
            {
                int opt = DisplayDataMenu();
                switch (opt)
                {
                    case 1:
                        Console.Clear();
                        // Layout of Titles for Data
                        Console.WriteLine(string.Format("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10}",
                            "Type", "ID", "Water", "Cost", "Weight(kg)", "Colour", "Milk/Wool"));
                        foreach (var itemList in LMS)
                        {
                            item = itemList.ToString();
                            Console.WriteLine(item);
                        }
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        break;
                    case 3:
                        // Return to the main menu
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private static int DisplayDataMenu() // Display Menu to Print the Data and Statistics
        {
            int opt = 0;
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine("*************** [ Data & Statistics ] ****************");
                Console.WriteLine("*                                                    *");
                Console.WriteLine("*             1. Display LMS Data                    *");
                Console.WriteLine("*             2. Display LMS Statistics              *");
                Console.WriteLine("*             3. Exit to Main Menu                   *");
                Console.WriteLine("*                                                    *");
                Console.WriteLine("******************************************************");
                Console.WriteLine();
                Console.WriteLine("Enter an Option: ");

                try
                {
                    opt = int.Parse(Console.ReadLine());
                    if (opt >= 1 && opt <= 3)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid choice. Please try again.\n");
                    }
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please enter a valid option (1-3).\n");
                }
            }
            return opt;
        }
        #endregion

        #region QUERY DATA MENU / SWITCH
        public void QueryDataSwitch()
        {
            string item;
            while (true)
            {
                int opt = QueryDataMenu();
                switch (opt)
                {
                    case 1:
                        // Query by ID
                        Console.Clear();
                        QueryAnimalByID();
                        break;
                    case 2:
                        // Query by Type
                        Console.Clear();
                        QueryAnimalByType();
                        break;
                    case 3:
                        // Query by Colour
                        Console.Clear();
                        QueryAnimalByColour();
                        break;
                    case 4:
                        // Insert a record from the database.
                        Console.Clear();
                        break;
                    case 5:
                        // Delete a record into the database.
                        Console.Clear();
                        break;
                    case 6:
                        // Return to the main menu
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }

        private static int QueryDataMenu() // Query Menu to Insert/Delete & Find Data by User Input
        {
            int opt = 0;
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine("****************** [ Query Menu ] ********************");
                Console.WriteLine("*                                                    *");
                Console.WriteLine("*                 1. Query By ID                     *");
                Console.WriteLine("*                 2. Query By Type                   *");
                Console.WriteLine("*                 3. Query By Colour                 *");
                Console.WriteLine("*                 4. Insert a Record                 *");
                Console.WriteLine("*                 5. Delete a Record                 *");
                Console.WriteLine("*                 6. Update a Record                 *");
                Console.WriteLine("*                 7. Exit to Main Menu               *");
                Console.WriteLine("*                                                    *");
                Console.WriteLine("******************************************************");
                Console.WriteLine();
                Console.WriteLine("Enter an Option: ");

                try
                {
                    opt = int.Parse(Console.ReadLine());
                    if (opt >= 1 && opt <= 7)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid choice. Please try again.\n");
                    }
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please enter a valid option (1-7).\n");
                }
            }
            return opt;
        }
        #endregion

        #endregion

        #region ----------------------------------------------------------------------------------- [ LIVE STOCK MANAGEMENT - METHODS ] --------------------
        internal abstract class LiveStockManagement
        {
            public string Type { get; set; }

            public LiveStockManagement(string type)
            {
                this.Type = type;
            }
        }

        internal abstract class Animals : LiveStockManagement
        {
            public int ID { get; set; }

            public string Colour { get; set; }

            public Animals(string type, int id, string colour) : base(type)
            {
                this.ID = id;
                this.Colour = colour;
            }
        }

        internal class Cow : Animals
        {
            public double Water { get; set; }
            public int Cost { get; set; }
            public int Weight { get; set; }
            public double Milk { get; set; }
            public Cow(string type, int id, double water, int cost, int weight, string colour, double milk) : base(type, id, colour)
            {
                this.Water = water;
                this.Cost = cost;
                this.Weight = weight;
                this.Milk = milk;
            }
            public override string ToString()
            {
                return string.Format("{0,-20} {1,-10} {2,-10:C} {3,-10:C} {4,-10} {5,-10} {6,-10:C}", // Adjust the widths as needed
                    this.GetType().Name, this.ID, this.Water, this.Cost, this.Weight, this.Colour, this.Milk
                );
            }
        }

        internal class Goat : Animals
        {
            public double Water { get; set; }
            public int Cost { get; set; }
            public int Weight { get; set; }
            public double Milk { get; set; }
            public Goat(string type, int id, double water, int cost, int weight, string colour, double milk) : base(type, id, colour)
            {
                this.Water = water;
                this.Cost = cost;
                this.Weight = weight;
                this.Milk = milk;
            }
            public override string ToString()
            {
                return string.Format("{0,-20} {1,-10} {2,-10:C} {3,-10:C} {4,-10} {5,-10} {6,-10:C}", // Adjust the widths as needed
                    this.GetType().Name, this.ID, this.Water, this.Cost, this.Weight, this.Colour, this.Milk
                );
            }
        }

        internal class Sheep : Animals
        {
            public double Water { get; set; }
            public double Cost { get; set; }
            public double Weight { get; set; }
            public double Wool { get; set; }
            public Sheep(string type, int id, double water, double cost, double weight, string colour, double wool) : base(type, id, colour)
            {
                this.Water = water;
                this.Cost = cost;
                this.Weight = weight;
                this.Wool = wool;
            }
            public override string ToString()
            {
                return string.Format("{0,-20} {1,-10} {2,-10:C} {3,-10:C} {4,-10} {5,-10} {6,-10:C}", // Adjust the widths as needed
                    this.GetType().Name, this.ID, this.Water, this.Cost, this.Weight, this.Colour, this.Wool
                );
            }
        }

        internal class Commodity : LiveStockManagement
        {
            public string Item { get; set; }
            public double Price { get; set; }
            public Commodity(string type, string item, double price) : base(type)
            {
                this.Item = item;
                this.Price = price;
            }
            public override string ToString()
            {
                return string.Format("{0,-20} {1,27:C}", // Adjust the widths as needed
                    this.Item,
                    this.Price
                );
            }
        }
        #endregion

        #region ----------------------------------------------------------------------------------- [ DATABASE METHODS ] -----------------------------------

        internal void ReadDB() // Read the Database
        {
            using (var cmd = Conn.CreateCommand())
            {
                cmd.Connection = Conn;
                string sql;
                OdbcDataReader reader;

                #region ------------------------------------------------------------------ [   COW TABLE   ] ---------------------------------------------
                sql = "SELECT * FROM Cow";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string type = GetType().Name;
                    int id = Util.GetInt(reader["ID"]);
                    double water = Util.GetDouble(reader["Water"]);
                    int cost = Util.GetInt(reader["Cost"]);
                    int weight = Util.GetInt(reader["Weight"]);
                    string colour = Util.GetString(reader["Colour"]);
                    double milk = Util.GetDouble(reader["Milk"]);
                    if (id == Util.BAD_INT ||
                        water == Util.BAD_DOUBLE ||
                        cost == Util.BAD_INT ||
                        weight == Util.BAD_INT ||
                        colour == Util.BAD_STRING ||
                        milk == Util.BAD_DOUBLE)
                    {
                        Console.WriteLine("Bad Row Detected");
                        continue; // Corrupted Row, Skips
                    }
                    var cow = new Cow(type, id, water, cost, weight, colour, milk);
                    LMS.Add(cow);
                }
                reader.Close();
                #endregion

                #region ------------------------------------------------------------------ [   GOAT TABLE   ] ---------------------------------------------
                sql = "SELECT * FROM Goat";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string type = GetType().Name;
                    int id = Util.GetInt(reader["ID"]);
                    double water = Util.GetDouble(reader["Water"]);
                    int cost = Util.GetInt(reader["Cost"]);
                    int weight = Util.GetInt(reader["Weight"]);
                    string colour = Util.GetString(reader["Colour"]);
                    double milk = Util.GetDouble(reader["Milk"]);
                    if (id == Util.BAD_INT ||
                        water == Util.BAD_DOUBLE ||
                        cost == Util.BAD_INT ||
                        weight == Util.BAD_INT ||
                        colour == Util.BAD_STRING ||
                        milk == Util.BAD_DOUBLE)
                    {
                        Console.WriteLine("Bad Row Detected");
                        continue; // Corrupted Row, Skips
                    }
                    var goat = new Goat(type, id, water, cost, weight, colour, milk);
                    LMS.Add(goat);
                }
                reader.Close();
                #endregion

                #region ------------------------------------------------------------------ [   SHEEP TABLE   ] ---------------------------------------------
                sql = "SELECT * FROM Sheep";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string type = GetType().Name;
                    int id = Util.GetInt(reader["ID"]);
                    double water = Util.GetDouble(reader["Water"]);
                    double cost = Util.GetDouble(reader["Cost"]);
                    double weight = Util.GetDouble(reader["Weight"]);
                    string colour = Util.GetString(reader["Colour"]);
                    double wool = Util.GetDouble(reader["Wool"]);
                    if (id == Util.BAD_INT ||
                        water == Util.BAD_DOUBLE ||
                        cost == Util.BAD_DOUBLE ||
                        weight == Util.BAD_DOUBLE ||
                        colour == Util.BAD_STRING ||
                        wool == Util.BAD_DOUBLE)
                    {
                        Console.WriteLine("Bad Row Detected");
                        continue; // Corrupted Row, Skips
                    }
                    var sheep = new Sheep(type, id, water, cost, weight, colour, wool);
                    LMS.Add(sheep);
                }
                reader.Close();
                #endregion

                #region ------------------------------------------------------------------ [   COMMODITY TABLE   ] -------------------------------------------
                sql = "SELECT * FROM Commodity";
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string type = GetType().Name;
                    string item = Util.GetString(reader["Item"]);
                    double price = Util.GetDouble(reader["Price"]);
                    if (item == Util.BAD_STRING ||
                        price == Util.BAD_DOUBLE)
                    {
                        Console.WriteLine("Bad Row Detected");
                        continue; // Corrupted Row, Skips
                    }
                    var commodity = new Commodity(type, item, price);
                    LMS.Add(commodity);
                }
                reader.Close();
                #endregion

            }
        }

        internal void QueryAnimalByID()
        {
            Console.WriteLine("Enter the ID of the animal you want to query:");
            if (int.TryParse(Console.ReadLine(), out int input))
            {
                var animal = LMS.FirstOrDefault(a => a is Animals && ((Animals)a).ID == input);
                if (animal != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Animal with the ID: [" + input + "] Found.");
                    Console.WriteLine();
                    Console.WriteLine(string.Format("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10}",
                                                    "Type", "ID", "Water", "Cost", "Weight(kg)", "Colour", "Milk/Wool"));
                    Console.WriteLine(animal);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("\nAnimal not found with the specified ID.");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid integer ID.");
                Console.WriteLine();
            }
        }

        internal void QueryAnimalByType()
        {
            Console.WriteLine("Enter the Type of the animal you want to query:");
            string inputType = Console.ReadLine();
            var animalsOfType = LMS.Where(a => a is Animals && string.Equals(a.GetType().Name, inputType, StringComparison.OrdinalIgnoreCase)).ToList();

            if (animalsOfType.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Animals with Type: [" + inputType + "] Found.");
                Console.WriteLine();
                Console.WriteLine(string.Format("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10}",
                                                "Type", "ID", "Water", "Cost", "Weight(kg)", "Colour", "Milk/Wool"));
                foreach (var animal in animalsOfType)
                {
                    Console.WriteLine(animal);
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\nNo animals found with the specified Type.");
                Console.WriteLine();
            }
        }

        internal void QueryAnimalByColour()
        {
            Console.WriteLine("Enter the Colour of the animal you want to query:");
            string inputColour = Console.ReadLine();
            var animalsOfColour = LMS.Where(a => a is Animals && string.Equals(((Animals)a).Colour, inputColour, StringComparison.OrdinalIgnoreCase)).ToList();

            if (animalsOfColour.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Animals with Colour: [" + inputColour + "] Found.");
                Console.WriteLine();
                Console.WriteLine(string.Format("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10}",
                                                "Type", "ID", "Water", "Cost", "Weight(kg)", "Colour", "Milk/Wool"));
                foreach (var animal in animalsOfColour)
                {
                    Console.WriteLine(animal);
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\nNo animals found with the specified Colour.");
                Console.WriteLine();
            }
        }

        internal void QueryAnimalByWeight()
        {
            // To Do
        }

        //insert row
        public void InsertAnimal(Animals a)
        {
            using (var cmd = Conn.CreateCommand())
            {
                string sql;
                Console.WriteLine("What kind of animal would you like to insert?\n cow, goat, or sheep?");
                string type = Console.ReadLine();
                string tbl;
                if (type.Equals("Cow", StringComparison.OrdinalIgnoreCase))
                {
                    tbl = "Cow";
                    Cow cow = a as Cow;
                    if (cow != null)
                    {
                    sql = "INSERT INTO Cow (Water, Cost, Weight, Colour, Milk) " +
                         "VALUES (@Water, @Cost, @Weight, @Colour, @Milk)";
                    cmd.Parameters.AddWithValue("@Water", a.Water);
                    cmd.Parameters.AddWithValue("@Cost", a.Cost);
                    cmd.Parameters.AddWithValue("@Weight", a.Weight);
                    cmd.Parameters.AddWithValue("@Colour", a.Colour);
                    cmd.Parameters.AddWithValue("@Milk", a.Milk);

                    cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        Console.WriteLine("What kind of animal?");
                    }
                    
                }
                else if (type.Equals("Goat", StringComparison.OrdinalIgnoreCase))
                {
                    tbl = "Goat";
                    Goat goat = (Goat)a;
                    sql = "INSERT INTO Goat (Water, Cost, Weight, Colour, Milk) " +
                         "VALUES (@Water, @Cost, @Weight, @Colour, @Milk)";
                    cmd.Parameters.AddWithValue("@Water", a.Water);
                    cmd.Parameters.AddWithValue("@Cost", a.Cost);
                    cmd.Parameters.AddWithValue("@Weight", a.Weight);
                    cmd.Parameters.AddWithValue("@Colour", a.Colour);
                    cmd.Parameters.AddWithValue("@Milk", a.Milk);
                }
                else if (type.Equals("Sheep", StringComparison.OrdinalIgnoreCase))
                {
                    tbl = "Sheep";
                    Sheep sheep = (Sheep)a;
                    sql = "INSERT INTO Sheep (Water, Cost, Weight, Colour, Wool) " +
                         "VALUES (@Water, @Cost, @Weight, @Colour, @Wool)";
                    cmd.Parameters.AddWithValue("@Water", a.Water);
                    cmd.Parameters.AddWithValue("@Cost", a.Cost);
                    cmd.Parameters.AddWithValue("@Weight", a.Weight);
                    cmd.Parameters.AddWithValue("@Colour", a.Colour);
                    cmd.Parameters.AddWithValue("@Wool", a.Wool);
                }
                else
                {
                    Console.WriteLine("Invalid animal type, please try again.");
                }
            }

            // Collect input for the new animal, such as type, ID, water, cost, etc.
            //LiveStockManagement-> Animals-> Cow, Goat, Sheep
            //LivestockManagement-> Commodity
        }

        //delete row
        internal void DeleteAnimal(int animalID)
        {
            // Create a SQL DELETE statement and execute it using your OdbcConnection
            string sql = "DELETE FROM AnimalTable WHERE ID = @ID";

            using (var cmd = Conn.CreateCommand())
            {
                cmd.Parameters.AddWithValue("@ID", animalID);

                // Execute the DELETE command
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Animal with ID " + animalID + " deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Animal with ID " + animalID + " not found in the database.");
                }
            }
        }


        //update row
        internal void UpdateAnimal(int animalID, double newWeight)
        {
            // Create a SQL UPDATE statement and execute it using your OdbcConnection
            string sql = "UPDATE AnimalTable SET Weight = @NewWeight WHERE ID = @ID";

            using (var cmd = new OdbcCommand(sql, Conn))
            {
                cmd.Parameters.AddWithValue("@NewWeight", newWeight);
                cmd.Parameters.AddWithValue("@ID", animalID);

                // Execute the UPDATE command
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Animal with ID " + animalID + " updated successfully.");
                }
                else
                {
                    Console.WriteLine("Animal with ID " + animalID + " not found in the database.");
                }
            }
        }


        internal static class Util // Validate Data & Connection
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

            internal static OdbcConnection GetConn() // Connection to the Database
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