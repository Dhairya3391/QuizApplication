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
                command.Parameters.AddWithValue("@IsActive", true);
                command.Parameters.AddWithValue("@IsAdmin", false);
                command.Parameters.AddWithValue("@Created", DateTime.Now);
                command.Parameters.AddWithValue("@Modified", DateTime.Now);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public User LoginUser(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("PR_User_Login", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserID = reader.GetInt32("UserID"),
                            Username = reader.GetString("Username"),
                            Email = reader.GetString("Email"),
                            Mobile = reader.IsDBNull(reader.GetOrdinal("Mobile")) ? null : reader.GetString("Mobile"),
                            IsAdmin = reader.GetBoolean("IsAdmin")
                        };
                    }
                    return null;
                }
            }
        }
    }
}