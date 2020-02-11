using SIS.MvcFramework.Attributes.Validation;

namespace Musaca.App.ViewModels.Get.User
{
    public class LoginView
    {
        [RequiredSis]
        public string Username { get; set; }

        [RequiredSis]
        public string Password { get; set; }
    }
}
