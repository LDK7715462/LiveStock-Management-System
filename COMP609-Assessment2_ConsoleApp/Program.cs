using System.Data;

namespace COMP609_Assessment2_ConsoleApp;

    class Program
    {
        static void Main(string[] args)
        {
            // Initialize your database connection here if you're using one.
            // Example: SqlConnection conn = new SqlConnection("connection_string_here");

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
                            // Implement code to read data from the database.
                            // Example: ReadDataFromDatabase(conn);
                            break;
                        case 2:
                            // Implement code to display data.
                            break;
                        case 3:
                            // Implement code to display statistics.
                            break;
                        case 4:
                            // Implement code to query by ID/colour/livestock type/weight.
                            break;
                        case 5:
                            // Implement code to delete a record from the database.
                            break;
                        case 6:
                            // Implement code to insert a record into the database.
                            break;
                        case 7:
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

        // Define your database-related functions here.
        // Example: static void ReadDataFromDatabase(SqlConnection conn) { }
    }