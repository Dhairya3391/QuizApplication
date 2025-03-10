using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models;

public class QuizWiseQuestionModel
{
    public int QuizWiseQuestionsID { get; set; }

    [Required(ErrorMessage = "Quiz Name is required.")]
    public int QuizID { get; set; }

    [Required(ErrorMessage = "Question Text is required.")]
    public int QuestionID { get; set; }

    [Required(ErrorMessage = "User Name is required.")]
    public int UserID { get; set; }

    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}