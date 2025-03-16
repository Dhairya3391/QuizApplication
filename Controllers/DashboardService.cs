using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.InkML;
using QuizApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace QuizApplication.Services;

public class DashboardService
{
    private readonly string _connectionString;
    //private readonly IHttpContextAccessor _httpContextAccessor;

    private static IHttpContextAccessor _httpContextAccessor;

    public DashboardService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionString");
        _httpContextAccessor = new HttpContextAccessor();

    }

    public DashboardViewModel GetDashboardData()
    {
        var model = new DashboardViewModel();

        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            int? userId = SessionVariables.UserID();

            // Total Quizzes
            using (var command = new SqlCommand("SELECT COUNT(*) FROM dbo.MST_Quiz where MST_Quiz.UserID=@UserID ", connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);
                model.TotalQuizzes = (int)command.ExecuteScalar();
            }

            // Total Questions (Sum of TotalQuestions from MST_Quiz)
            using (var command = new SqlCommand("SELECT Count(*) FROM dbo.MST_Question where MST_Question.UserID=@UserID", connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);
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
    q.QuizName, 
    COUNT(que.QuestionID) AS QuestionCount
FROM dbo.MST_Quiz q
LEFT JOIN dbo.MST_Question que ON q.UserID = que.UserID
WHERE q.UserID = @UserID
GROUP BY q.QuizName
ORDER BY QuestionCount DESC;
", connection))
            {
                command.Parameters.AddWithValue("@UserID", userId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        model.RecentQuizAttempts.Add(new QuizAttemptData
                        {
                            QuizName = reader.GetString("QuizName"),
                            AttemptCount = reader.GetInt32("QuestionCount")
                        });
                }
            }
        }

        return model;
    }
}