using SIS.MvcFramework.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SULS.App.ViewModels
{
    public class UserRegisterViewModel
    {
        [RequiredSis]
        [StringLengthSis(5,20, "Lenght should be between 5 and 20 characters")]
        public string Username { get; set; }

        [RequiredSis]
        [EmailSis]
        public string Email { get; set; }

        [RequiredSis]
        [StringLengthSis(6,20, "Password should be between 6 and 20 characters")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
