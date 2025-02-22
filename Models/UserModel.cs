using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "User ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number.")]
        public int? UserID { get; set; }
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "password is required.")]
        public string Password { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "ConfirmPassword is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
