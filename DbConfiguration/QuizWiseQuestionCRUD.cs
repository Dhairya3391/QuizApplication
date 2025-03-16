using System.Data;
using System.Data.SqlClient;

namespace QuizApplication.DbConfiguration;

public class QuizWiseQuestionCRUD
{
    private readonly IConfiguration configuration;

    public QuizWiseQuestionCRUD(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public bool AddQuizWiseQuestionRef(int quizId)
    {
        string connectionString = configuration.GetConnectionString("ConnectionString");

        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand command = new SqlCommand("PR_QuizQuestionsWise_Insert", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@QuizID", quizId);
        command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
        int result = command.ExecuteNonQuery();
        return result > 0;
    }

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
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_QuizQuestionsWise_Details";
                command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
                // ✅ Pass quizId as a parameter to the stored procedure
                command.Parameters.AddWithValue("@QuizID", quizId);

                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }
    }

    public void SaveSelectedQuestions(int quizId, List<int> selectedQuestions)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            foreach (var questionId in selectedQuestions)
                using (var command =
                       new SqlCommand(
                           "INSERT INTO MST_QuizWiseQuestions (QuizID, QuestionID, UserID, Created, Modified) VALUES (@QuizID, @QuestionID, @UserID, GETDATE(), GETDATE())",
                           connection))
                {
                    command.Parameters.AddWithValue("@QuizID", quizId);
                    command.Parameters.AddWithValue("@QuestionID", questionId);
                    command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
                    command.ExecuteNonQuery();
                }
        }
    }

    public int GetQuizIdByName(string quizName)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new SqlCommand("SELECT QuizID FROM MST_Quiz WHERE QuizName = @QuizName and MST_Quiz.UserID=@UserID ", connection))
            {
                command.Parameters.AddWithValue("@QuizName", quizName);
                command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0; // Return 0 if not found
            }
        }
    }


    //public List<int> GetAddedQuestionIds(int quizId)
    //{
    //    var questionIds = new List<int>();
    //    var connectionString = configuration.GetConnectionString("ConnectionString");

    //    using (var connection = new SqlConnection(connectionString))
    //    {
    //        connection.Open();
    //        using (var command = new SqlCommand("SELECT QuestionID FROM MST_QuizWiseQuestions WHERE QuizID = @QuizID",
    //                   connection))
    //        {
    //            command.Parameters.AddWithValue("@QuizID", quizId);

    //            using (var reader = command.ExecuteReader())
    //            {
    //                while (reader.Read()) questionIds.Add(reader.GetInt32(0));
    //            }
    //        }
    //    }

    //    return questionIds;
    //}
    public List<int> GetAddedQuestionIds(int quizId)
    {
        var questionIds = new List<int>();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT ISNULL(QuestionID, 0) FROM MST_QuizWiseQuestions WHERE QuizID = @QuizID", connection))
            {
                command.Parameters.AddWithValue("@QuizID", quizId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0)) // Ensure it's not NULL before adding
                        {
                            questionIds.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
        }

        return questionIds;
    }


    //public bool AddQuizWiseQuestion(int quizId, int questionId, int userId)
    //{
    //    var connectionString = configuration.GetConnectionString("ConnectionString");

    //    using (var connection = new SqlConnection(connectionString))
    //    {
    //        connection.Open();
    //        using (var command = new SqlCommand("PR_QuizQuestionsWise_Insert", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.AddWithValue("@QuizID", quizId);
    //            command.Parameters.AddWithValue("@QuestionID", questionId);
    //            command.Parameters.AddWithValue("@UserID", 1);

    //            var result = command.ExecuteNonQuery();
    //            return result > 0;
    //        }
    //    }
    //}
    public bool AddQuizWiseQuestion(int quizId, int questionId, int userId)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var deleteCommand = new SqlCommand("DELETE FROM MST_QuizWiseQuestions WHERE QuizID = @QuizID AND QuestionID IS NULL", connection, transaction))
                    {
                        deleteCommand.Parameters.AddWithValue("@QuizID", quizId);
                        deleteCommand.ExecuteNonQuery();
                    }

                    using (var insertCommand = new SqlCommand("PR_QuizQuestionsWise_Insert", connection, transaction))
                    {
                        insertCommand.CommandType = CommandType.StoredProcedure;
                        insertCommand.Parameters.AddWithValue("@QuizID", quizId);
                        insertCommand.Parameters.AddWithValue("@QuestionID", questionId);
                        insertCommand.Parameters.AddWithValue("@UserID", SessionVariables.UserID());

                        var result = insertCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return result > 0;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }


    public bool DeleteQuizWiseQuestion(int quizWiseQuestionId)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = new SqlCommand("PR_QuizQuestionsWise_DeleteByPk", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());

        command.Parameters.AddWithValue("@QuizWiseQuestionsID", quizWiseQuestionId);
        var result = command.ExecuteNonQuery();
        return result > 0;
    }
}