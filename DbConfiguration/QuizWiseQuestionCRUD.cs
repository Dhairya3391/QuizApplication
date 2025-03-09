using System.Data;
using System.Data.SqlClient;

namespace QuizApplication.DbConfiguration
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

        public void SaveSelectedQuestions(int quizId, List<int> selectedQuestions)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (int questionId in selectedQuestions)
                {
                    using (SqlCommand command = new SqlCommand("INSERT INTO MST_QuizWiseQuestions (QuizID, QuestionID, UserID, Created, Modified) VALUES (@QuizID, @QuestionID, @UserID, GETDATE(), GETDATE())", connection))
                    {
                        command.Parameters.AddWithValue("@QuizID", quizId);
                        command.Parameters.AddWithValue("@QuestionID", questionId);
                        command.Parameters.AddWithValue("@UserID", 1); // Replace with actual user ID
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public int GetQuizIdByName(string quizName)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT QuizID FROM MST_Quiz WHERE QuizName = @QuizName", connection))
                {
                    command.Parameters.AddWithValue("@QuizName", quizName);

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0; // Return 0 if not found
                }
            }
        }


        public List<int> GetAddedQuestionIds(int quizId)
        {
            List<int> questionIds = new List<int>();
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT QuestionID FROM MST_QuizWiseQuestions WHERE QuizID = @QuizID", connection))
                {
                    command.Parameters.AddWithValue("@QuizID", quizId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questionIds.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
            return questionIds;
        }

        public bool AddQuizWiseQuestion(int quizId, int questionId, int userId)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_QuizQuestionsWise_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@QuizID", quizId);
                    command.Parameters.AddWithValue("@QuestionID", questionId);
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@Created", DateTime.Now);
                    command.Parameters.AddWithValue("@Modified", DateTime.Now);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public bool DeleteQuizWiseQuestion(int quizWiseQuestionId)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("PR_QuizQuestionsWise_DeleteByPk", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuizWiseQuestionsID", quizWiseQuestionId);
            int result = command.ExecuteNonQuery();
            return result > 0;
        }


    }
}
