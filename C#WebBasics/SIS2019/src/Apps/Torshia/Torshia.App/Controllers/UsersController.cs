using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;
using Torshia.App.ViewModels.PostMethodModels;
using Torshia.Services;

namespace Torshia.App.Controllers
{
    public class UsersController:Controller
    {

        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterModel model)
        {
            var modelStateIsValid = this.ModelState.IsValid;
            var passwordMatchesRepeatPassword = model.Password == model.ConfirmPassword;
            var thereIsADuplicate = this.userService.CheckForDuplicateUsernameOrEmail(model.Username, model.Email);

            if (!modelStateIsValid || !passwordMatchesRepeatPassword || thereIsADuplicate)
            {
                return this.Register();
            }

            this.userService.CreateUser(model.Username, model.Email, model.Password);
            return this.Redirect("/Users/Login");
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginModel model)
        {
            var modelStateIsValid = this.ModelState.IsValid;
            var userExists = this.userService.CheckIfUserExists(model.Username, model.Password);

            if (!modelStateIsValid || !userExists)
            {
                return this.Login();
            }

            var user = this.userService.GetUserByUserName(model.Username);

            this.SignIn(user.Id, user.Username, user.Email,user.Role.ToString());

            return this.Redirect("/");

        }

        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
