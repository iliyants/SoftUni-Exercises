using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.ViewModels.Get.User
{
    public class RegisterView
    {
        [RequiredSis]
        [StringLengthSis(3, 20, "Username should be between 3 and 20 characters long !")]
        public string Username { get; set; }

        [RequiredSis]
        [StringLengthSis(4,15, "Password should be between 4 and 15 characters long !")]
        public string Password { get; set; }

        [RequiredSis]
        [StringLengthSis(3, 20, "Username should be between 3 and 20 characters long !")]
        public string ConfirmPassword { get; set; }

        [RequiredSis]
        [EmailSis]
        public string Email { get; set; }
    }
}
