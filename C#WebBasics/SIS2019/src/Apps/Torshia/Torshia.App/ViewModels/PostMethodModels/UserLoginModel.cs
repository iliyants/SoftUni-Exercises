using SIS.MvcFramework.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Torshia.App.ViewModels.PostMethodModels
{
    public class UserLoginModel
    {
        [RequiredSis]
        [StringLengthSis(3, 20, "Username length must be between 3 and 20 characters")]
        public string Username { get; set; }

        [RequiredSis]
        [StringLengthSis(5, 20, "Password Length must be beween 5 and 20 characters")]
        public string Password { get; set; }
    }
}
