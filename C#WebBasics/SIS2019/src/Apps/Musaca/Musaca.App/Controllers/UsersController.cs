using Musaca.App.ViewModels.Get.Receipt;
using Musaca.App.ViewModels.Get.User;
using Musaca.Services;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;

namespace Musaca.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IReceiptService receiptService;

        public UsersController(IUserService userService, IReceiptService receiptService)
        {
            this.userService = userService;
            this.receiptService = receiptService;
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userId = this.User.Id;

            var allReceipts = this.receiptService.GetAllReceiptsByUser(userId);

            var model = allReceipts
                .Where(x => x.ReceiptOrders.All(order => order.Order.Status.ToString() == "Completed"))
                .Select(x => new ReceiptAllViewModel()
                {
                    Id = x.Id,
                    Cashier = x.Cashier.Username,
                    IssuedOn = x.IssuedOn,
                    Total = x.ReceiptOrders
                    .Select(s => s.Order.Quantity * s.Order.OrderProducts.Sum(p => p.Product.Price)).Sum()
                }).ToList();

            return this.View(model);
        }
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginView model)
        {
            var succesfullLogin = this.userService.SuccesfullLogin(model.Username, model.Password);

            if (!succesfullLogin || !ModelState.IsValid)
            {
                return this.Login();
            }

            var user = this.userService.GetUserByUserName(model.Username);

            this.SignIn(user.Id, user.Username, user.Email, user.Role.ToString());

            return this.Redirect("/");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterView model)
        {
            var passwordsMatch = model.Password == model.ConfirmPassword;
            var uniqueUsernameAndEmail = this.userService.UsernameAndEmailAreUnique(model.Username, model.Email);

            if (!ModelState.IsValid || !passwordsMatch || !uniqueUsernameAndEmail)
            {
                return this.Register();
            }

            var user = this.userService.CreateUser(model.Username, model.Password, model.Email);
            this.SignIn(user.Id, user.Username, user.Email, user.Role.ToString());

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
