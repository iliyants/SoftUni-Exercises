using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels;
using SULS.Services;
using System.Collections.Generic;
using System.Linq;

namespace SULS.App.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly IProblemService problemService;
        private readonly ISubmissionService submissionService;

        public ProblemsController(IProblemService problemService, ISubmissionService submissionService)
        {
            this.problemService = problemService;
            this.submissionService = submissionService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ProblemCreateViewModel model)
        {
            var isValidModelState = this.ModelState.IsValid;
            var isDuplicateProblemName = this.problemService.CheckForDuplicateProblemNames(model.Name);

            if (!isValidModelState || isDuplicateProblemName)
            {
                return this.Create();
            }

            this.problemService.CreateProblem(model.Name, model.Points);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Details()
        {
            var problemId = ((ISet<string>)this.Request.QueryData["id"]).FirstOrDefault();
            var problem = this.problemService.GetProblemById(problemId);

            var submissionsFromDb = this.submissionService.GetAllSubmissionsByProblem(problemId);

            var submissions = submissionsFromDb.Select(x => new ProblemDetailsViewModel()
            {
                CreatedOn = x.CreatedOn,
                Username = x.User.Username,
                AchievedResult = x.AchievedResult,
                MaxPoints = problem.Points,
                SubmissionId = x.Id
            }).ToList();

            var viewModel = new ProblemDetailsCollectionViewModel()
            {
                ProblemSubmissions = new KeyValuePair<string, List<ProblemDetailsViewModel>>(problem.Name, submissions)
            };


            return this.View(viewModel);
        }
    }
}

