using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models;

public class QuestionLevelsModel
{
    public int QuestionLevelID { get; set; }

    [Required(ErrorMessage = "Question Level is required.")]
    public string QuestionLevel { get; set; }
    public int UserID { get; set; }

    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}