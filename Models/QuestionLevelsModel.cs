using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class QuestionLevelsModel
    {
        public int QuestionLevelID { get; set; }

        [Required(ErrorMessage = "Question Level is required.")]
        public string QuestionLevel { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number.")]
        public int UserID { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
