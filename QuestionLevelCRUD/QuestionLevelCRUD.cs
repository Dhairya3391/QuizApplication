using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.QuestionLevelCRUD
{
    public class QuestionLevelCRUD
    {
        private IConfiguration configuration;
        
        public QuestionLevelCRUD(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        
        public bool AddQuestionLevel(QuestionLevelsModel model)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_QuestionLevel_Insert", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@QuestionLevel", model.QuestionLevel);
                command.Parameters.AddWithValue("@UserID", model.UserID);
                command.Parameters.AddWithValue("@Created", DateTime.Now);
                command.Parameters.AddWithValue("@Modified", DateTime.Now);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
        
        public DataTable EditQuestionLevel(int questionLevelId)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_QuestionLevel_SelectByPK", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@QuestionLevelID", questionLevelId);

                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }
        
        public bool UpdateQuestionLevel(QuestionLevelsModel model)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_QuestionLevel_UpdateByPk", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@QuestionLevelID", model.QuestionLevelID);
                command.Parameters.AddWithValue("@QuestionLevel", model.QuestionLevel);
                command.Parameters.AddWithValue("@UserID", model.UserID);
                command.Parameters.AddWithValue("@Created", model.Created);
                command.Parameters.AddWithValue("@Modified", DateTime.Now);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeleteQuestionLevel(int questionLevelId)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_QuestionLevel_DeleteByPk", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@QuestionLevelID", questionLevelId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
}