//using System.ComponentModel.DataAnnotations;

//namespace QuizApplication.Models
//{
//    public class QuestionModel
//    {
//        public int QuestionID { get; set; }

//        [Required(ErrorMessage = "Question text is required")]
//        public string QuestionText { get; set; }
//        public int QuestionLevelID { get; set; }

//        public string OptionA { get; set; }        
//        public string OptionB { get; set; }
//        public string OptionC { get; set; }
//        public string OptionD { get; set; }
//        public string CorrectOption { get; set; }
//        public int QuestionMarks { get; set; }
//        public bool IsActive { get; set; }
//        public int UserID { get; set; }
//        public int Created { get; set; }
//        public int Modified { get; set; }
//    }
//}


using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class QuestionModel
    {
        public int QuestionID { get; set; }

        [Required(ErrorMessage = "Please enter the question text.")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Please select the question level.")]
        public int QuestionLevelID { get; set; }

        [Required(ErrorMessage = "Option A is required.")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required.")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = "Option C is required.")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = "Option D is required.")]
        public string OptionD { get; set; }

        [Required(ErrorMessage = "Please select the correct option.")]
        public string CorrectOption { get; set; }

        [Required(ErrorMessage = "Question marks are required.")]
        public int? QuestionMarks { get; set; }

        [Required(ErrorMessage = "Please specify if the question is active.")]
        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }

        public int Created { get; set; }
        public int Modified { get; set; }
    }
}
