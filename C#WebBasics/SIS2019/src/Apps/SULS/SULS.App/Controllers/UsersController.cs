using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels;
using SULS.Services;


namespace SULS.App.Controllers
{
    public class UsersController:Controller
    {

        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginViewModel model)
        {
            if (!this.userService.CheckForValidUsernameAndPassword(model.Username,model.Password))
            {
                return this.Login();
            }

            var user = this.userService.GetUserByUsername(model.Username);
            string blank = "";
            this.SignIn(user.Id, user.Username, user.Email, blank);

            return this.Redirect("/");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterViewModel model)
        {
            var validModelState = this.ModelState.IsValid;
            var matchingPasswords = model.Password == model.ConfirmPassword;
            var isDuplicateUser = this.userService.CheckForDuplicateUsers(model.Username, model.Email);

            if (!validModelState || !matchingPasswords || isDuplicateUser)
            {
                return this.Register();
            }
         
            this.userService.CreateUser(model.Username, model.Email, model.Password);

            return this.Login();
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }


     
    }
}
