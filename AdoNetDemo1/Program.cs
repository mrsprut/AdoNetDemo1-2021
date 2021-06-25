using System;
using System.Data.SqlClient;
using System.Threading;

// using System.Security.Permissions;

namespace AdoNetDemo1
{
    class Program
    {
        private static readonly string CONNECTION_STRING =
            "Data Source=192.168.0.106,1433;Initial Catalog=AdoNetDemo;Integrated Security=False;user id=sa;password=Passw0rd%";

        // private static readonly string QUEUE_NAME = "AklContentToteChangeMessages";
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection(CONNECTION_STRING);
            SomeMethod(connection);
        }
        
        static void Initialization(string connectionString/*, string queueName*/)
        {
            // Create a dependency connection.
            SqlDependency.Start(connectionString/*, queueName*/);
        }

        static void SomeMethod(SqlConnection connection)
        {
            try
            {
                Initialization(CONNECTION_STRING/*, QUEUE_NAME*/);
                connection.Open();
                // Assume connection is an open SqlConnection.
                // Create a new SqlCommand object.
                using var command = new SqlCommand(
                    "SELECT [id], [name] FROM [dbo].[Demo]",
                    connection
                );
                // Create a dependency and associate it with the SqlCommand.
                SqlDependency dependency = new SqlDependency(command);
                // Maintain the reference in a class member.
                // Subscribe to the SqlDependency event.
                dependency.OnChange += OnDependencyChange;
                // Execute the command.
                using SqlDataReader reader = command.ExecuteReader();
                // Process the DataReader.
                while (reader.Read())
                {
                    Console.WriteLine($"id: {reader[0]}; name: {reader[1]}");
                }
                while (true)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Listening...");
                }
            }
            catch (Exception e)
            {
                Termination(CONNECTION_STRING/*, QUEUE_NAME*/);
            }
        }

        // Handler method
        static void OnDependencyChange(object sender, SqlNotificationEventArgs ev)
        {
            // Handle the event (for example, invalidate this cache entry).
            Console.WriteLine($"Event Args: {ev.Info} {ev.Source} {ev.Type}");
            if (ev.Info.ToString() == "Insert")
            {
                using SqlConnection connection = new SqlConnection(CONNECTION_STRING);
                connection.Open();
                using var command = new SqlCommand(
                    "SELECT [id], [name] FROM [dbo].[Demo] WHERE [id] = IDENT_CURRENT('Demo')",
                    connection
                );
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"id: {reader[0]}; name: {reader[1]}");
                }
            }
        }

        static void Termination(string connectionString/*, string queueName*/)
        {
            // Release the dependency.
            SqlDependency.Stop(connectionString/*, queueName*/);
        }
    }
}