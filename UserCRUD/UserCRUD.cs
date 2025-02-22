using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.UserCRUD
{
    public class UserCRUD
    {
        private IConfiguration configuration;
        

        public UserCRUD(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public bool AddQuiz(UserModel model)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("PR_User_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserID", model.UserID);
            command.Parameters.AddWithValue("@UserName", model.UserName);
            command.Parameters.AddWithValue("@Password", model.Password);
            command.Parameters.AddWithValue("@ConfirmPassword", model.ConfirmPassword);
            command.Parameters.AddWithValue("@Email", model.Email);
            command.Parameters.AddWithValue("@MobileNo", model.MobileNo);
            command.Parameters.AddWithValue("@Created", model.Created);
            command.Parameters.AddWithValue("@Modified", model.Modified);
            int result = command.ExecuteNonQuery();
            return result > 0;

        }
    }
}
