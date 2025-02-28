using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.UserCrud
{
    public class UserCrud
    {
        private readonly string _connectionString;

        public UserCrud(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentNullException(nameof(_connectionString), "Connection string 'DefaultConnection' not found.");
            }
        }

        public bool RegisterUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("PR_User_Register", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Mobile", user.Mobile ?? (object)DBNull.Value);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}