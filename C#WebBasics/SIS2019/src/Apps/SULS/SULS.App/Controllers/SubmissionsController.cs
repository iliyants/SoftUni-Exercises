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
    public class SubmissionsController:Controller
    {
        private readonly IProblemService problemService;
        private readonly ISubmissionService submissionService;

        public SubmissionsController(IProblemService problemService, ISubmissionService submissionService)
        {
            this.problemService = problemService;
            this.submissionService = submissionService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var problemId = ((ISet<string>)this.Request.QueryData["id"]).FirstOrDefault();

            var problem = this.problemService.GetProblemById(problemId);
            var model = new SubmissionCreateViewModel()
            {
                ProblemId = problem.Id,
                ProblemName = problem.Name
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(SubmissionCreateViewModelPOST model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Create();
            }

            var userId = this.User.Id;
            var problemId = ((ISet<string>)this.Request.FormData["ProblemId"]).FirstOrDefault();

            var problem = this.problemService.GetProblemById(problemId);

            this.submissionService.CreateSubmission(model.Code, problem.Points, problemId, userId);
            this.problemService.IncrementProblemSubmissions(problemId);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Delete()
        {
            var submissionId = ((ISet<string>)this.Request.QueryData["id"]).FirstOrDefault();
            var problemId = this.submissionService.GetProblemIdBySubmissionId(submissionId);
            this.problemService.DecrementPoblemSubmissions(problemId);
            this.submissionService.DeleteSubmission(submissionId);

            return this.Redirect("/");

        }
    }
}
