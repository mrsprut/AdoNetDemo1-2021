using System;
using System.Data.SqlClient;

namespace AdoNetDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SqlConnection connection = new SqlConnection("Data Source=192.168.0.106,1433;Initial Catalog=AdoNetDemo;Integrated Security=False;user id=sa;password=Passw0rd%");
            connection.Open();
            SqlCommand select = connection.CreateCommand();
            select.CommandText = "SELECT * FROM Demo";
            SqlDataReader reader =
                select.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"id: {reader[0]}; name: {reader[1]}");
            }
            reader.Close();
            connection.Close();
        }
    }
}