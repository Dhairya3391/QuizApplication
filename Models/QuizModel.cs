using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class QuizModel
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime QuizDate { get; set; }
        public int UserID { get; set; }
        public DateTime Created { get; set; }
        public string Modified { get; set; }
    }
}
