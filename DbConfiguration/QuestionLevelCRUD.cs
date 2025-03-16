using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.DbConfiguration;

public class QuestionLevelCRUD
{
    private readonly IConfiguration configuration;

    public QuestionLevelCRUD(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public bool AddQuestionLevel(QuestionLevelsModel model)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_QuestionLevel_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuestionLevel", model.QuestionLevel);
            command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
            command.Parameters.AddWithValue("@Created", DateTime.Now);
            command.Parameters.AddWithValue("@Modified", DateTime.Now);

            var result = command.ExecuteNonQuery();
            return result > 0;
        }
    }

    public DataTable EditQuestionLevel(int questionLevelId)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_QuestionLevel_SelectByPK", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
            command.Parameters.AddWithValue("@QuestionLevelID", questionLevelId);

            var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }
    }

    public bool UpdateQuestionLevel(QuestionLevelsModel model)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_QuestionLevel_UpdateByPk", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuestionLevelID", model.QuestionLevelID);
            command.Parameters.AddWithValue("@QuestionLevel", model.QuestionLevel);
            command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
            command.Parameters.AddWithValue("@Created", model.Created);
            command.Parameters.AddWithValue("@Modified", DateTime.Now);

            var result = command.ExecuteNonQuery();
            return result > 0;
        }
    }

    public bool DeleteQuestionLevel(int questionLevelId)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_QuestionLevel_DeleteByPk", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
            command.Parameters.AddWithValue("@QuestionLevelID", questionLevelId);

            var result = command.ExecuteNonQuery();
            return result > 0;
        }
    }
}