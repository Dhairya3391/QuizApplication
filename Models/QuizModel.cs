using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models;

public class QuizModel
{
    public int QuizID { get; set; }

    [Required(ErrorMessage = "Quiz Name is required.")]
    public string QuizName { get; set; }

    [Required(ErrorMessage = "Total Questions are required.")]
    [Range(1, 500, ErrorMessage = "Total Questions must be between 1 and 500.")]
    public int TotalQuestions { get; set; }

    [Required(ErrorMessage = "Quiz Date is required.")]
    [DataType(DataType.Date)]
    public DateTime QuizDate { get; set; }

    [Required(ErrorMessage = "User ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number.")]
    public int UserID { get; set; }

    public DateTime Created { get; set; }

    //public string Modified { get; set; }
    public DateTime Modified { get; set; }
}