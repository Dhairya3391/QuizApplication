using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.DbConfiguration;

public class UserCrud
{
    private readonly string _connectionString;
    private IConfiguration _configuration;

    public UserCrud(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionString");
        _configuration = configuration;
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

            try
            {
                var rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error: {ex.Message}", ex);
            }
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
                    return new User
                    {
                        UserID = reader.GetInt32("UserID"),
                        Username = reader.GetString("Username"),
                        Email = reader.GetString("Email"),
                        Mobile = reader.IsDBNull(reader.GetOrdinal("Mobile")) ? null : reader.GetString("Mobile"),
                        IsAdmin = reader.GetBoolean("IsAdmin")
                    };
                return null;
            }
        }
    }

    public DataTable Users(string spName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = spName;
   
                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }
    }
    public bool DeleteUser(int userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_User_DeleteByPk", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", userId);

            var result = command.ExecuteNonQuery();
            return result > 0;
        }
    }
}