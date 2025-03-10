using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.DbConfiguration;

public class QuizCRUD
{
    private readonly IConfiguration configuration;

    public QuizCRUD(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public bool AddQuiz(QuizModel model)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_Quiz_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuizName", model.QuizName);
            command.Parameters.AddWithValue("@TotalQuestions", model.TotalQuestions);
            command.Parameters.AddWithValue("@QuizDate", model.QuizDate);
            command.Parameters.AddWithValue("@UserID", model.UserID);
            command.Parameters.AddWithValue("@Created", DateTime.Now);
            command.Parameters.AddWithValue("@Modified", DateTime.Now);

            var result = command.ExecuteNonQuery();
            return result > 0;
        }
    }

    public DataTable EditQuiz(int quizId)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_Quiz_SelectByID", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@QuizID", quizId);

            var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }
    }

    public bool UpdateQuiz(QuizModel model)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_Quiz_UpdateByPk", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuizID", model.QuizID);
            command.Parameters.AddWithValue("@QuizName", model.QuizName);
            command.Parameters.AddWithValue("@TotalQuestions", model.TotalQuestions);
            command.Parameters.AddWithValue("@QuizDate", model.QuizDate);
            command.Parameters.AddWithValue("@UserID", model.UserID);
            command.Parameters.AddWithValue("@Created", model.Created);
            command.Parameters.AddWithValue("@Modified", model.Modified);

            var result = command.ExecuteNonQuery();
            return result > 0;
        }
    }

    public bool DeleteQuiz(int quizId)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_Quiz_DeleteByPk", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@QuizID", quizId);

            var result = command.ExecuteNonQuery();
            return result > 0;
        }
    }

    public DataTable SearchQuizzes(string quizName, int? totalQuestion, DateTime? quizDate)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_Quiz_Search", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuizName", (object)quizName ?? DBNull.Value);
            command.Parameters.AddWithValue("@TotalQuestion", (object)totalQuestion ?? DBNull.Value);
            command.Parameters.AddWithValue("@QuizDate", (object)quizDate ?? DBNull.Value);

            var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }
    }
}