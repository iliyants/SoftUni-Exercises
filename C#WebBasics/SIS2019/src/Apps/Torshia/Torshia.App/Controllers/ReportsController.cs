using SIS.MvcFramework;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System.Linq;
using Torshia.App.ViewModels.GetMethodModels;
using Torshia.Services;

namespace Torshia.App.Controllers
{
    public class ReportsController:Controller
    {
        private readonly IReportService reportService;
        private readonly ITaskService taskService;

        public ReportsController(IReportService reportService, ITaskService taskService)
        {
            this.reportService = reportService;
            this.taskService = taskService;
        }

        [Authorize]
        public IActionResult All()
        {
            var tasks = this.taskService.GetAllTasks();

            var model = tasks.Select(x => new TaskReportsViewModel()
            {
                Id = x.Id,
                TaskName = x.Title,
                TaskLevel = x.AffectedSectors.Count(),
                TaskStatus = this.reportService.GetReportSatusByTask(x.Id) ?? "No reports",
            }).ToList();

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            var userId = this.User.Id;

            this.reportService.CreateReport(userId, id);
            this.taskService.ReportATask(id);

            return this.Redirect("/");
        }
    }
}
