using System.ComponentModel.DataAnnotations;

namespace BrockAllen.MembershipReboot.Mvc.Areas.UserAccount.Models
{
    public class RegisterInputModel
    {
        [ScaffoldColumn(false)]
        public bool EmailIsUsername { get; set; }

        [StringLength(254)]
        public string Username { get; set; }

        [Required, EmailAddress, StringLength(254)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Password confirmation must match password."), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}