using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Mapping;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels;
using SULS.Services;
using System.Linq;

namespace SULS.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProblemService problemService;

        public HomeController(IProblemService problemService)
        {
            this.problemService = problemService;
        }


        [HttpGet(Url = "/")]
        public IActionResult Slash()
        {
            return this.Index();
        }

        public IActionResult Index()
        {

            var viewModel = this.problemService.GetAllProblems().Select(x =>
                new ProblemAllViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Submissions = x.Submissions

                });

            return this.View(viewModel);
        }

    }
}
