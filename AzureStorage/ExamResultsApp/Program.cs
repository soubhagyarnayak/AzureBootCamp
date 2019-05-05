namespace ExamResultApp
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            const string Init = "init";
            const string View = "view";

            while (true)
            {
                Console.WriteLine($"Please enter the command: '{Init}' for initializing database and '{View}' for viewing the result for a student.");
                string command = Console.ReadLine();
                if (command.Equals(Init, StringComparison.OrdinalIgnoreCase))
                {
                    ExamResultsTable.Initialize();
                    Console.WriteLine("Storage have been populated with student data.");
                }
                else if (command.Equals(View, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Enter StudentId or StudentId Subject for fetching required result.");
                    var clientInput = Console.ReadLine().Split(' ');
                    if (clientInput.Length == 1)
                    {
                        var results = ExamResultsTable.GetResult(clientInput[0]);
                        if (results.Count == 0)
                        {
                            Console.WriteLine("There is no associated entry in the database.");
                        }
                        else
                        {
                            Console.WriteLine("Subject          Score");
                            foreach (var result in results)
                            {
                                Console.WriteLine($"{result.RowKey} {result.Score}");
                            }
                        }
                    }
                    else
                    {
                        var result = ExamResultsTable.GetResult(clientInput[0],clientInput[1]);
                        if(result == null)
                        {
                            Console.WriteLine("There is no associated entry in the database.");
                        }
                        else
                        {
                            Console.WriteLine($"The score is: {result.Score}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{command} is not a valid command");
                }
            }
        }
    }


}
