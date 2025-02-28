using System.Data.SqlClient;
using System.Data;

namespace QuizApplication.DbConfigruation
{
    public class DbConfiguration
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public DbConfiguration(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = GetWorkingConnectionString();
        }

        private string GetWorkingConnectionString()
        {
            string[] connectionStrings =
            {
                configuration.GetConnectionString("ConnectionString"),
                configuration.GetConnectionString("ConnectionString1"),
                configuration.GetConnectionString("ConnectionString2"),
                configuration.GetConnectionString("ConnectionString3")
            };

            foreach (var conn in connectionStrings)
            {
                if (string.IsNullOrEmpty(conn)) continue; // Skip empty connection strings

                try
                {
                    using (var connection = new SqlConnection(conn))
                    {
                        connection.Open();
                        connection.Close();
                        Console.WriteLine($"Connected successfully to: {conn}");
                        return conn; // Return first working connection
                    }
                }
                catch
                {
                    Console.WriteLine($"Failed to connect using: {conn}");
                }
            }

            throw new Exception("No valid database connection found!");
        }

        public DataTable GetAllData(string spName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = spName;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        return table;
                    }
                }
            }
        }
    }
}
