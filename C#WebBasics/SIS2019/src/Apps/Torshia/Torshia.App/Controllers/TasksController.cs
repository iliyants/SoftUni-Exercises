using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using Torshia.App.ViewModels.GetMethodModels;
using Torshia.App.ViewModels.PostMethodModels;
using Torshia.Services;

namespace Torshia.App.Controllers
{
    public class TasksController:Controller
    {
        private readonly ITaskService taskService;
        private readonly IUserService userService;
        private readonly ISectorService sectorService;
        private readonly IReportService reportService;

        public TasksController(ITaskService taskService, IUserService userService, ISectorService sectorService, IReportService reportService)
        {
            this.taskService = taskService;
            this.userService = userService;
            this.sectorService = sectorService;
            this.reportService = reportService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(TaskCreateModel model)
        {

            var modelStateIsValid = this.ModelState.IsValid;
            var allUsersExist = this.userService.ChecksForExistingUsersByUsernames(model.Participants);

            if (!modelStateIsValid || !allUsersExist)
            {
                return this.Create();
            }

            var viewSectors = new List<string>()
            {
                model.Customers,
                model.Finances,
                model.Management,
                model.Internal,
                model.Marketing
            }.Where(x => x != null).ToList();


            this.sectorService.CreateSectorsIfTheyDoesntExist(viewSectors);
            this.taskService.CreateTask(model.Title, model.DueDate, model.Description, model.Participants, viewSectors);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var task = this.taskService.GetTaskById(id);


            var model = new UserTaskDetailsViewModel()
            {
                Level = task.Level,
                Title = task.Title,
                DueDate = task.DueDate,
                Participants = String.Join(", ", task.Participants),
                AffectedSectors = String.Join(", ", task.AffectedSectors),
                Description = task.Description
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult DetailsAdmin(string id)
        {

            var task = this.taskService.GetTaskById(id);

            var model = new AdminTaskDetailsViewModel()
            {
                ReportId = this.reportService.GetReportIdByTaskId(task.Id),
                TaskName = task.Title,
                Level = task.Level,
                TaskStatus = this.reportService.GetReportSatusByTask(task.Id),
                DueDate = task.DueDate,
                ReportedOn = this.reportService.GetReportDateByTaskId(task.Id),
                Reporter = this.reportService.GetReporterNameByTaskId(task.Id),
                Participants = String.Join(", ", task.Participants.ToList()),
                AffectedSectors = String.Join(", ", task.AffectedSectors.ToList()),
                TaskDescription = task.Description
            };

            return this.View(model);
        }

    }
}
