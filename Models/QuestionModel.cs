using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class QuestionModel
    {
        public int QuestionID { get; set; }

        public string QuestionText { get; set; }
        public int QuestionLevelID { get; set; }

        public string OptionA { get; set; }        
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectOption { get; set; }
        public int QuestionMarks { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }
        public int Created { get; set; }
        public int Modified { get; set; }
    }
}
