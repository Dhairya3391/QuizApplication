using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class QuizWiseQuestionModel
    {
        public int QuizWiseQuestionsID { get; set; }
        public int QuizID { get; set; }
        public int QuestionID { get; set; }
        public int UserID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
