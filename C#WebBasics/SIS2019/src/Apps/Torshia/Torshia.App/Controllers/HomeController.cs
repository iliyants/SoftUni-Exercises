using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;
using System.Linq;
using Torshia.App.ViewModels.GetMethodModels;
using Torshia.Services;

namespace Torshia.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaskService taskService;

        public HomeController(ITaskService taskService)
        {
            this.taskService = taskService;
        }


        [HttpGet(Url = "/")]
        public IActionResult Slash()
        {
            return this.Index();
        }
        public IActionResult Index()
        {

            var viewModel = this.taskService
                .GetAllTasks()
                .Where(x => !x.IsReported)
                .Select(x => new IndexTasksViewModel()
            {
                Id = x.Id,
                Name = x.Title,
                Level = x.AffectedSectors.Count()
            }).ToList();


            return this.View(viewModel);

        }
    }
}
