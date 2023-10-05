using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        #endregion

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
    }
}