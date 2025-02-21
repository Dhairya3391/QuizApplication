using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.QuestionCRUD
{
    public class QuestionCRUD
    {
        private IConfiguration configuration;

        public QuestionCRUD(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public bool AddQuiz(QuizModel model)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("PR_Quiz_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuizName", model.QuizName);
            command.Parameters.AddWithValue("@TotalQuestions", model.TotalQuestions);
            command.Parameters.AddWithValue("@QuizDate", model.QuizDate);
            command.Parameters.AddWithValue("@UserID", 1);
            int result = command.ExecuteNonQuery();
            return result > 0;

        }
    }
}
