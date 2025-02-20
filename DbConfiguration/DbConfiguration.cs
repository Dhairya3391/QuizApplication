using System.Data.SqlClient;
using System.Data;
using QuizApplication.Models;
namespace QuizApplication.DbConfigruation
{
    public class DbConfiguration
    {
        private IConfiguration configuration;

        public DbConfiguration(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public DataTable GetAllData(String spName)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = spName;

            SqlDataReader reader = command.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }
    }
}
