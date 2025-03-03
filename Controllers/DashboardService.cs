using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.Services
{
    public class DashboardService
    {
        private readonly string _connectionString;

        public DashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        public DashboardViewModel GetDashboardData()
        {
            var model = new DashboardViewModel();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Total Quizzes
                using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.MST_Quiz", connection))
                {
                    model.TotalQuizzes = (int)command.ExecuteScalar();
                }

                // Total Questions (Sum of TotalQuestions from MST_Quiz)
                using (var command = new SqlCommand("SELECT SUM(TotalQuestions) FROM dbo.MST_Quiz", connection))
                {
                    model.TotalQuestions = command.ExecuteScalar() as int? ?? 0; // Handle NULL case
                }

                // Total Users
                using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.MST_User", connection))
                {
                    model.TotalUsers = (int)command.ExecuteScalar();
                }

                // Top 4 Quizzes by Question Count (Using TotalQuestions)
                using (var command = new SqlCommand(
                    @"SELECT TOP 4 
                        QuizName, 
                        TotalQuestions as QuestionCount 
                      FROM dbo.MST_Quiz 
                      ORDER BY TotalQuestions DESC", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.RecentQuizAttempts.Add(new QuizAttemptData
                            {
                                QuizName = reader.GetString("QuizName"),
                                AttemptCount = reader.GetInt32("QuestionCount")
                            });
                        }
                    }
                }
            }

            return model;
        }
    }
}