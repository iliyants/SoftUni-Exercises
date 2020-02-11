using SIS.MvcFramework.Attributes.Validation;

namespace Torshia.App.ViewModels.PostMethodModels
{
    public class UserRegisterModel
    {
        [RequiredSis]
        [StringLengthSis(3,20, "Username length must be between 3 and 20 characters")]
        public string Username { get; set; }

        [RequiredSis]
        [EmailSis]
        public string Email { get; set; }

        [RequiredSis]
        [StringLengthSis(5,20, "Password Length must be beween 5 and 20 characters")]
        public string Password { get; set; }

        [RequiredSis]
        [StringLengthSis(5, 20, "Password Length must be beween 5 and 20 characters")]
        public string ConfirmPassword { get; set; }
    }
}
