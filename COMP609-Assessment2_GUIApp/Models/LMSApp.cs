using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP609_Assessment2_GUIApp.Models
{
    internal class LMSApp
    {
        public ObservableCollection<LiveStockManagement> LMS { get; set; }
        public ObservableCollection<Animals> Animal { get; set; }

        OdbcConnection Conn;

        public LMSApp()
        {
            LMS = new ObservableCollection<LiveStockManagement>();
            Animal = new ObservableCollection<Animals>();
            Conn = Util.GetConn();
            ReadDB();
        }

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
                    Animal.Add(cow);
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
                    Animal.Add(goat);
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
                    Animal.Add(sheep);
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

        #region ANIMAL QUERIES
        internal void QueryAnimalByID()
        {
            Console.WriteLine("Enter the ID of the animal you want to query:");
            if (int.TryParse(Console.ReadLine(), out int input))
            {
                var animal = Animal.FirstOrDefault(a => a is Animals && ((Animals)a).ID == input);
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
            var animalsOfType = Animal.Where(a => a is Animals && string.Equals(a.GetType().Name, inputType, StringComparison.OrdinalIgnoreCase)).ToList();

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
            var animalsOfColour = Animal.Where(a => a is Animals && string.Equals(((Animals)a).Colour, inputColour, StringComparison.OrdinalIgnoreCase)).ToList();

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
            Console.WriteLine("Enter the weight (kg) of the animal you want to query:");

            if (double.TryParse(Console.ReadLine(), out double inputWeight))
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Find animals with weight greater than the specified value.");
                Console.WriteLine("2. Find animals with weight less than the specified value.");
                Console.Write("Enter your choice (1 or 2): ");

                if (int.TryParse(Console.ReadLine(), out int option) && (option == 1 || option == 2))
                {
                    var animalsByWeight = Animal
                        .Where(a => a is Animals)
                        .ToList();

                    if (option == 1)
                    {
                        animalsByWeight = animalsByWeight
                            .Where(a => ((Animals)a).Weight >= inputWeight)
                            .ToList();
                        Console.WriteLine($"Animals with Weight Greater than {inputWeight} kg Found:");
                    }
                    else if (option == 2)
                    {
                        animalsByWeight = animalsByWeight
                            .Where(a => ((Animals)a).Weight <= inputWeight)
                            .ToList();
                        Console.WriteLine($"Animals with Weight Less than {inputWeight} kg Found:");
                    }

                    if (animalsByWeight.Any())
                    {
                        Console.WriteLine();
                        Console.WriteLine(string.Format("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10}",
                                                        "Type", "ID", "Water", "Cost", "Weight(kg)", "Colour", "Milk/Wool"));
                        foreach (var animal in animalsByWeight)
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
                        Console.WriteLine($"No animals found with the specified Weight.");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid option. Please enter 1 for greater than or 2 for less than.");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid numeric weight.");
                Console.WriteLine();
            }
        }
        #endregion

        #region INSERT ANIMAL
        public void ConsoleInsertDB()
        {
            Console.WriteLine("======[ Insert Animal ]======");

            // Read the animal type from the user
            string animalType = ReadAnimalType();

            // Generate a new ID
            int id = GetNewID();

            Animals animal;
            string colour;
            double water, milkOrWool;
            int cost, weight;

            switch (animalType.ToLower())
            {
                case "cow":
                    water = GetValidDoubleInput("Water: ");
                    cost = GetValidIntInput("Cost: ");
                    weight = GetValidIntInput("Weight: ");
                    colour = ReadColour();
                    milkOrWool = GetValidDoubleInput("Milk: ");
                    animal = new Cow("Cow", id, water, cost, weight, colour, milkOrWool);
                    break;
                case "goat":
                    water = GetValidDoubleInput("Water: ");
                    cost = GetValidIntInput("Cost: ");
                    weight = GetValidIntInput("Weight: ");
                    colour = ReadColour();
                    milkOrWool = GetValidDoubleInput("Milk: ");
                    animal = new Goat("Goat", id, water, cost, weight, colour, milkOrWool);
                    break;
                case "sheep":
                    water = GetValidDoubleInput("Water: ");
                    cost = GetValidIntInput("Cost: ");
                    weight = GetValidIntInput("Weight: ");
                    colour = ReadColour();
                    milkOrWool = GetValidDoubleInput("Milk: ");
                    animal = new Sheep("Sheep", id, water, cost, weight, colour, milkOrWool);
                    break;
                default:
                    Console.WriteLine("Invalid animal type.");
                    return;
            }
            if (!InsertAnimal(animal))
                Console.WriteLine("Insertion didn't go through.");
            else
                Console.WriteLine($"\nRecord Inserted: {animal}\n");
        }

        private string ReadAnimalType()
        {
            List<string> validAnimalTypes = new List<string> { "cow", "goat", "sheep" };
            var textInfo = CultureInfo.CurrentCulture.TextInfo;

            while (true)
            {
                Console.Write("Enter animal type (Cow, Goat, or Sheep): ");
                string animalType = Console.ReadLine()?.Trim().ToLower();

                if (validAnimalTypes.Contains(animalType))
                {
                    return textInfo.ToTitleCase(animalType);
                }
                else
                {
                    Console.WriteLine("\nInvalid animal type. Valid options are: Cow, Goat, Sheep.\n");
                }
            }
        }

        private string ReadColour()
        {
            List<string> validColors = new List<string> { "red", "black", "white" };

            while (true)
            {
                Console.Write("Enter colour (Red, Black, or White): ");
                string colour = Console.ReadLine().Trim().ToLower();

                if (validColors.Contains(colour))
                {
                    // Capitalize the first letter and return the formatted color
                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                    return textInfo.ToTitleCase(colour);
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Valid options are: Red, Black, or White.\n");
                }
            }
        }

        public bool InsertAnimal(Animals a)
        {
            int numRows;
            using (var cmd = Conn.CreateCommand())
            {
                string sql;
                string tbl = a.GetType().Name;

                if (tbl.Equals("Cow"))
                {
                    Animals animal = (Animals)a;
                    sql = "INSERT INTO Cow ([ID],[Water], [Cost], [Weight], [Colour], [Milk]) " +
                         "VALUES (?,?,?,?,?,?)";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@ID", animal.ID);
                    cmd.Parameters.AddWithValue("@Water", animal.Water);
                    cmd.Parameters.AddWithValue("@Cost", animal.Cost);
                    cmd.Parameters.AddWithValue("@Weight", animal.Weight);
                    cmd.Parameters.AddWithValue("@Colour", animal.Colour);
                    cmd.Parameters.AddWithValue("@Milk", animal.Wool_Milk);
                }
                else if (tbl.Equals("Goat"))
                {
                    Animals animal = (Animals)a;
                    sql = "INSERT INTO Goat ([ID],[Water], [Cost], [Weight], [Colour], [Milk]) " +
                         "VALUES (?,?,?,?,?,?)";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@ID", animal.ID);
                    cmd.Parameters.AddWithValue("@Water", animal.Water);
                    cmd.Parameters.AddWithValue("@Cost", animal.Cost);
                    cmd.Parameters.AddWithValue("@Weight", animal.Weight);
                    cmd.Parameters.AddWithValue("@Colour", animal.Colour);
                    cmd.Parameters.AddWithValue("@Milk", animal.Wool_Milk);
                }
                else if (tbl.Equals("Sheep"))
                {
                    Animals animal = (Animals)a;
                    sql = "INSERT INTO Sheep ([ID],[Water], [Cost], [Weight], [Colour], [Wool]) " +
                         "VALUES (?,?,?,?,?,?)";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@ID", animal.ID);
                    cmd.Parameters.AddWithValue("@Water", animal.Water);
                    cmd.Parameters.AddWithValue("@Cost", animal.Cost);
                    cmd.Parameters.AddWithValue("@Weight", animal.Weight);
                    cmd.Parameters.AddWithValue("@Colour", animal.Colour);
                    cmd.Parameters.AddWithValue("@Milk", animal.Wool_Milk);
                }
                numRows = CommitDB(cmd);
            }
            if (numRows == 1)
            {
                Animal.Add(a);
                return true;
            }
            return false;
        }

        public int GetNewID()
        {
            return Animal.Max(x => x.ID) + 1;
        }
        #endregion

        #region DELETE ANIMAL BY ID
        // Delete Animal by ID
        public void ConsoleDeleteByID()
        {
            Console.WriteLine("======[ Delete Record ]======");
            int id = ConsoleGetID();
            var c = GetObjectByID(id);
            if (c == null)
            {
                Console.WriteLine($"Non-existent id: {id}");
                return;
            }

            string tbl = c.GetType().Name;
            if (DeleteByID(tbl, id))
                Console.WriteLine($"Record deleted: {c}\n");
            else
                Console.WriteLine($"Error in deleting {id}\n");

        }

        public bool DeleteByID(string tbl, int id)
        {
            int numRows;
            using (var cmd = Conn.CreateCommand())
            {
                string sql = $"DELETE FROM {tbl} WHERE [ID] =?";
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@ID", id);
                numRows = CommitDB(cmd);

                // Execute the DELETE command
                if (numRows == 1)
                {
                    // Successfully deletes from the database
                    var animal = Animal.FirstOrDefault(x => x.ID == id);
                    if (animal != null)
                    {
                        Console.WriteLine("Animal with ID: " + id + " deleted successfully.");
                        return Animal.Remove(animal);
                    }
                    else
                    {
                        Console.WriteLine("Animal with ID: " + id + " not found in the database.");
                    }
                }
                return false;
            }
        }

        public int ConsoleGetID()
        {
            int id;
            while (true)
            {
                Console.WriteLine("Enter id: ");
                string? s = Console.ReadLine();
                if (int.TryParse(s, out id))
                    break;
                else
                    Console.WriteLine("Invalid Input. Try Again");
            }
            return id;
        }

        public Animals? GetObjectByID(int id)
        {
            // Retrieves objects info using id
            return Animal.FirstOrDefault(x => x.ID == id);
        }
        #endregion

        #region UPDATE ANIMAL
        // Update Animal
        internal void UpdateAnimalId()
        {
            Console.WriteLine("Enter the ID of the animal you want to update below");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                // Find the animal with the given ID using FirstOrDefault
                var animal = Animal.FirstOrDefault(a => a is Animals && ((Animals)a).ID == id);

                if (animal != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Animal with the ID: [{id}] Found.");
                    Console.WriteLine();
                    Console.WriteLine(string.Format("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10}",
                                                  "Type", "ID", "Water", "Cost", "Weight(kg)", "Colour", "Milk/Wool"));
                    Console.WriteLine(animal);
                }
                else
                {
                    Console.WriteLine($"Animal with [{id}] not found.");
                    Console.WriteLine();
                    return; // Exit the method if the animal is not found
                }

                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Update water consumption");
                Console.WriteLine("2. Update tax cost");
                Console.WriteLine("3. Update animal's weight");
                Console.WriteLine("4. Update Milk production");
                Console.WriteLine("5. Update Wool production");
                Console.Write("\nEnter your choice (1, 2, 3, 4, or 5): ");

                if (int.TryParse(Console.ReadLine(), out int option) && (option >= 1 && option <= 5))
                {
                    string columnName = "";
                    object newValue = null;

                    if (option == 1 || option == 2 || option == 3 || option == 4 || option == 5)
                    {
                        // For options 1, 2, 3, 4, or 5, expect a numeric input
                        columnName = option == 1 ? "Water" : (option == 2 ? "Cost" : (option == 3 ? "Weight" : (option == 4 ? "Milk" : "Wool")));
                        Console.Clear();
                        Console.Write($"Enter the new value for {columnName}: ");
                        if (double.TryParse(Console.ReadLine(), out double numValue))
                        {
                            newValue = numValue;
                        }
                        else
                        {
                            Console.WriteLine($"\nInvalid {columnName} value. Update failed.\n");
                            return;
                        }
                    }

                    string sql = $"UPDATE {animal.GetType().Name} SET [{columnName}] = ? WHERE [ID] = ?";
                    using (var cmd = new OdbcCommand(sql, Conn))
                    {
                        cmd.Parameters.AddWithValue("@Value", newValue);
                        cmd.Parameters.AddWithValue("@ID", id);
                        int rowsUpdated = cmd.ExecuteNonQuery();

                        if (rowsUpdated > 0)
                        {
                            // Update the corresponding object in the list
                            if (animal is Animals updatedAnimal)
                            {
                                if (columnName == "Water")
                                {
                                    updatedAnimal.Water = (double)newValue;
                                }
                                else if (columnName == "Cost")
                                {
                                    updatedAnimal.Cost = (double)newValue;
                                }
                                else if (columnName == "Weight")
                                {
                                    updatedAnimal.Weight = (double)newValue;
                                }
                                else if (columnName == "Milk")
                                {
                                    updatedAnimal.Wool_Milk = (double)newValue;
                                }
                                else if (columnName == "Wool")
                                {
                                    updatedAnimal.Wool_Milk = (double)newValue;
                                }
                            }

                            Console.WriteLine($"\nColumn: {columnName} for ID: [{id}] has been updated successfully.\n");
                        }
                        else
                        {
                            Console.WriteLine($"Update failed. No rows were affected.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid option. Please choose again");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a valid ID");
                Console.WriteLine();
            }
        }

        #endregion

        #region validation

        private int GetValidIntInput(string prompt)
        {
            int result;
            bool isValidInput = false;

            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out result))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer value.");
                }
            } while (!isValidInput);

            return result;
        }

        private double GetValidDoubleInput(string prompt)
        {
            double result;
            bool isValidInput = false;

            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (double.TryParse(input, out result))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric value.");
                }
            } while (!isValidInput);

            return result;
        }

        public int CommitDB(OdbcCommand cmd)
        {
            try
            {
                cmd.Transaction = Conn.BeginTransaction();
                int NumRowsAffected = cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
                return NumRowsAffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (cmd.Transaction != null)
                    cmd.Transaction.Rollback();
                return 0;
            }
        }
        #endregion

        #region ---------------------------- [ STATISTICS ] --------------------------------

        #region STATS SWITCH & MENU

        public void DisplayStatsSwitch()
        {
            while (true)
            {
                var opt = DisplayStatsMenu();
                switch (opt)
                {
                    case 1:
                        Console.Clear();
                        DailyStats();
                        break;
                    case 2:
                        Console.Clear();
                        GlobalStats();
                        break;
                    case 3:
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("Invalid Input.");
                        break;
                }
            }
        }
        public int DisplayStatsMenu()
        {
            int opt = 0;
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine("**************** [ Statistics Menu ] *****************");
                Console.WriteLine("*                                                    *");
                Console.WriteLine("*             1. Display Daily Statistics            *");
                Console.WriteLine("*             2. Display Global Statistics           *");
                Console.WriteLine("*             3. Back to Main Menu                   *");
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

        #region DAILY STATS
        private void DailyStats()
        {
            double goatMilkPrice = 0.0, cowMilkPrice = 0.0, sheepWoolPrice = 0.0, waterPrice = 0.0, liveStockWeightTax = 0.0;

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
                            Console.WriteLine($"Current Cows Milk price (KG): ${cowMilkPrice}");
                            break;
                        case "GoatMilk":
                            goatMilkPrice = itemPrice;
                            Console.WriteLine($"Current Goat Milk price (KG): ${goatMilkPrice}");
                            break;
                        case "SheepWool":
                            sheepWoolPrice = itemPrice;
                            Console.WriteLine($"Current Sheeps wool price (KG): ${sheepWoolPrice}");
                            break;
                        case "Water":
                            waterPrice = itemPrice;
                            Console.WriteLine($"Current cost of water (KG): ${waterPrice}");
                            break;
                        case "LivestockWeightTax":
                            liveStockWeightTax = itemPrice;
                            Console.WriteLine($"Current govt tax (KG per animal per day): ${liveStockWeightTax}");
                            break;
                    }
                }
                reader.Close();
            }

            Console.WriteLine("\nEnter the ID of the animal you want statistics for: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var animal = Animal.FirstOrDefault(a => a is Animals && ((Animals)a).ID == id);

                if (animal != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"ID No {id} has been found.");
                    Console.WriteLine();
                    Console.WriteLine(string.Format("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10} {6,-10}",
                                                  "Type", "ID", "Water", "Cost", "Weight(kg)", "Colour", "Milk/Wool"));
                    Console.WriteLine(animal);
                    Console.WriteLine($"\nShowing daily statistics for {animal.GetType().Name} with ID No {id}: \n");

                    if (animal.GetType().Name == "Cow")
                    {
                        double cowIncome = animal.Wool_Milk * cowMilkPrice;
                        //Note: livestock weight tax is per kg, not per animal
                        double cowCost = animal.Cost + (animal.Water * waterPrice) + (liveStockWeightTax * animal.Weight);
                        double cowProfit = cowIncome - cowCost;
                        Console.WriteLine($"Total Income per day: ${cowIncome:F2}");
                        Console.WriteLine($"Total Cost per day: ${cowCost:F2}");
                        Console.WriteLine($"Total profit/ loss per day ($): {cowProfit:F2}");

                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else if (animal.GetType().Name == "Goat")
                    {
                        double goatIncome = animal.Wool_Milk * goatMilkPrice;
                        //Note: livestock weight tax is per kg, not per animal
                        double goatCost = animal.Cost + (animal.Water * waterPrice) + (liveStockWeightTax * animal.Weight);
                        double goatProfit = goatIncome - goatCost;
                        Console.WriteLine($"Total Income per day: ${goatIncome:F2}");
                        Console.WriteLine($"Total Cost per day: ${goatCost:F2}");
                        Console.WriteLine($"Total profit/ loss per day ($): {goatProfit:F2}");

                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        double sheepIncome = animal.Wool_Milk * sheepWoolPrice;
                        //Note: livestock weight tax is per kg, not per animal
                        double sheepCost = animal.Cost + (animal.Water * waterPrice) + (liveStockWeightTax * animal.Weight);
                        double sheepProfit = sheepIncome - sheepCost;
                        Console.WriteLine($"Total Income per day: ${sheepIncome:F2}");
                        Console.WriteLine($"Total Cost per day: ${sheepCost:F2}");
                        Console.WriteLine($"Total profit/ loss per day ($): {sheepProfit:F2}");

                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"ID No [{id}] not found in database, please try again.");
                    Console.WriteLine();
                    DailyStats();
                    return;
                }
            }
        }
        #endregion

        #region GLOBAL STATS
        internal static OdbcConnection GetConn()
        {
            string? dbstr = ConfigurationManager.AppSettings.Get("odbcString");
            string fpath = @"..\..\FarmData.accdb";
            string connstr = dbstr + fpath;
            var conn = new OdbcConnection(connstr);
            conn.Open();
            return conn;
        }

        public void GlobalStats()
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
                Console.WriteLine("******[Current Wholesale Commodity Prices]******\n");

                while (reader.Read())
                {
                    string itemName = reader["Item"].ToString();
                    double itemPrice = Convert.ToDouble(reader["Price"]);
                    switch (itemName)
                    {
                        case "CowMilk":
                            cowMilkPrice = itemPrice;
                            Console.WriteLine($"Cow's Milk price (KG): ${cowMilkPrice}");
                            break;
                        case "GoatMilk":
                            goatMilkPrice = itemPrice;
                            Console.WriteLine($"Goat Milk price (KG): ${goatMilkPrice}");
                            break;
                        case "SheepWool":
                            sheepWoolPrice = itemPrice;
                            Console.WriteLine($"Sheep's wool price (KG): ${sheepWoolPrice}");
                            break;
                        case "Water":
                            waterPrice = itemPrice;
                            Console.WriteLine($"Cost of water (KG): ${waterPrice}");
                            break;
                        case "LivestockWeightTax":
                            liveStockWeightTax = itemPrice;
                            Console.WriteLine($"Government Livestock Weight Tax (KG per animal per day): ${liveStockWeightTax}");
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
                Console.WriteLine();
                Console.WriteLine("********************[Animal Statistics Totals]***********************");
                Console.WriteLine();
                totalTax = liveStockWeightTax * totalWeight;//tax per day
                Console.WriteLine($"Total Tax for all animals per day: ${totalTax:F2}");
                totalTax = liveStockWeightTax * totalWeight * 30; //tax per 30 days
                Console.WriteLine($"Total Tax for all animals per 30 day period: ${totalTax:F2}");
                double avgWeight = totalWeight / animalCount;// avg weight of all animals
                Console.WriteLine($"Total average weight of all animals in the Database: {avgWeight:F2} KG");
                Console.WriteLine();
                Console.WriteLine("******************************************************");
                Console.WriteLine();
                double totalIncome = (cowMilk * cowMilkPrice) + (sheepWool * sheepWoolPrice) + (goatMilk * goatMilkPrice);
                Console.WriteLine($"Total income from all animals in the database: ${totalIncome:F2}");
                //         operation cost of each cow, sheep & goat + water usage price for each cow, sheep & goat + total tax by weight for each animal in the db
                double totalCost = (cowCost + sheepCost + goatCost) + (cowWater * waterPrice) + (sheepWater * waterPrice) + (goatWater * waterPrice) + totalTax;
                Console.WriteLine($"Total costs incurred daily from all animals in the database: ${totalCost:F2}");
                double totalProfit = totalIncome - totalCost;
                Console.WriteLine($"Total profit gained from all animals in the database: ${totalProfit:F2}");
                Console.WriteLine();
                Console.WriteLine("******************************************************");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}

    #endregion
#endregion
    }
}
