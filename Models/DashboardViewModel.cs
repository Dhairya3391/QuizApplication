namespace QuizApplication.Models
{
    public class DashboardViewModel
    {
        public int TotalQuizzes { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalUsers { get; set; }
        public List<QuizAttemptData> RecentQuizAttempts { get; set; } = new List<QuizAttemptData>();
    }

    public class QuizAttemptData
    {
        public string QuizName { get; set; }
        public int AttemptCount { get; set; } // Will represent question count per quiz
    }
}