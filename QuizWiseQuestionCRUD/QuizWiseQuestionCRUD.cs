using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using QuizApplication.Models;

namespace QuizApplication.QuizWiseQuestionCRUDCRUD
{
    public class QuizWiseQuestionCRUD
    {
        private IConfiguration configuration;

        public QuizWiseQuestionCRUD(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        //public bool AddQuiz(QuizWiseQuestionModel model)
        //{
        //    string connectionString = configuration.GetConnectionString("ConnectionString");

        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    SqlCommand command = new SqlCommand("PR_Quiz_Insert", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.AddWithValue("@QuizName", model.QuizName);
        //    command.Parameters.AddWithValue("@TotalQuestions", model.TotalQuestions);
        //    command.Parameters.AddWithValue("@QuizDate", model.QuizDate);
        //    command.Parameters.AddWithValue("@UserID", 1);
        //    int result = command.ExecuteNonQuery();
        //    return result > 0;
        //}

        //public DataTable GetQuizWiseQuestionDetails(QuizWiseQuestionModel model)
        //{
        //    string connectionString = configuration.GetConnectionString("ConnectionString");

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = connection.CreateCommand())
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "PR_QuizQuestionsWise_Details";

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                DataTable table = new DataTable();
        //                table.Load(reader);
        //                return table;
        //            }
        //        }
        //    }
        //}

        public DataTable GetQuizWiseQuestionDetails(int quizId)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_QuizQuestionsWise_Details";

                    // ✅ Pass quizId as a parameter to the stored procedure
                    command.Parameters.AddWithValue("@QuizID", quizId);

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
