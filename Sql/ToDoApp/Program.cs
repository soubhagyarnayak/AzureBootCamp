namespace ToDoApp
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;

    class Program
    {
        const string Add = "add";
        const string View = "view";
        static string ConnectionString = string.Empty;

        static void Main(string[] args)
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            EnsureSchema();
            while (true)
            {
                Console.WriteLine($"Please enter the command: '{Add}' for adding item and '{View}' for viewing the items.");
                string command = Console.ReadLine();
                if (command.Equals(Add, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Please enter the item to add:");
                    string item = Console.ReadLine();
                    if (item.Length != 0)
                    {
                        AddItem(item);
                    }
                }
                else if (command.Equals(View, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("TODO list items:");
                    var items = FetchItems();
                    foreach(var item in items)
                    {
                        Console.WriteLine(item);
                    }
                }
                else
                {
                    Console.WriteLine($"{command} is not a valid command");
                }
            }
        }

        static void AddItem(string item)
        {
            string insertCommandText = "INSERT INTO AzureBootCampToDoList (Item) VALUES (@Item)";
            var sqlCommand = new SqlCommand(insertCommandText, new SqlConnection(ConnectionString));
            sqlCommand.Parameters.Add(new SqlParameter("@Item", item));
            using (sqlCommand.Connection)
            {
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
            }
        }

        static List<string> FetchItems()
        {
            var result = new List<string>();
            string selectCommandText = "SELECT Id, Item FROM AzureBootCampToDoList";
            var sqlCommand = new SqlCommand(selectCommandText, new SqlConnection(ConnectionString));
            using (sqlCommand.Connection)
            {
                sqlCommand.Connection.Open();
                var dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["Id"];
                        string item = (string)dataReader["Item"];
                        result.Add($"{id}. {item}");
                    }
                }
            }
            return result;
        }

        static void EnsureSchema()
        {
            string createTableCommand = @"IF NOT EXISTS (
                                                SELECT *FROM sys.types WHERE NAME = 'AzureBootCampToDoList' AND is_table_type = 1
                                                )
                                        BEGIN
                                            CREATE TABLE AzureBootCampToDoList(
                                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                                Item NVARCHAR(1024) NOT NULL
                                        );
                                        END;";
            var sqlCommand = new SqlCommand(createTableCommand, new SqlConnection(ConnectionString));
            using (sqlCommand.Connection)
            {
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
