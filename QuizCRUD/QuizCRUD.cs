using System;
using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.QuizCRUD
{
    public class QuizCRUD
    {
        private IConfiguration configuration;

        public QuizCRUD(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public bool AddQuiz(QuizModel model)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_Quiz_Insert", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@QuizName", model.QuizName);
                command.Parameters.AddWithValue("@TotalQuestions", model.TotalQuestions);
                command.Parameters.AddWithValue("@QuizDate", model.QuizDate);
                command.Parameters.AddWithValue("@UserID", model.UserID);
                command.Parameters.AddWithValue("@Created", DateTime.Now);
                command.Parameters.AddWithValue("@Modified", DateTime.Now);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public DataTable EditQuiz(int quizId)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_Quiz_SelectByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuizID", quizId);

                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public bool UpdateQuiz(QuizModel model)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_Quiz_UpdateByPk", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@QuizID", model.QuizID);
                command.Parameters.AddWithValue("@QuizName", model.QuizName);
                command.Parameters.AddWithValue("@TotalQuestions", model.TotalQuestions);
                command.Parameters.AddWithValue("@QuizDate", model.QuizDate);
                command.Parameters.AddWithValue("@UserID", model.UserID);
                command.Parameters.AddWithValue("@Created", model.Created);
                command.Parameters.AddWithValue("@Modified", model.Modified);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeleteQuiz(int quizId)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_Quiz_DeleteByPk", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@QuizID", quizId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}
