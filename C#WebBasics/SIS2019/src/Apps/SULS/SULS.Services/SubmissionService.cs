using SULS.Data;
using SULS.Models;
using System;
using System.Linq;

namespace SULS.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly SULSDbContext context;

        public SubmissionService(SULSDbContext context)
        {
            this.context = context;
        }

        public void CreateSubmission(string code, int problemMaxPoints, string problemId, string userId)
        {
            var submission = new Submission()
            {
                Code = code,
                CreatedOn = DateTime.UtcNow,
                AchievedResult = GenerateRandomNumber(problemMaxPoints),
                ProblemId = problemId,
                UserId = userId
            };

            this.context.Submissions.Add(submission);
            this.context.SaveChanges();
        }
        private int GenerateRandomNumber(int maxValue)
        {
            Random r = new Random();
            return r.Next(0, maxValue); 
        }

        public IQueryable<Submission> GetAllSubmissionsByProblem(string problemId)
        {
            return this.context.Submissions.Where(x => x.ProblemId == problemId);
        }

        public void DeleteSubmission(string submissionId)
        {
            var submission = this.context.Submissions.SingleOrDefault(x => x.Id == submissionId);
            this.context.Submissions.Remove(submission);
            this.context.SaveChanges();
        }

        public string GetProblemIdBySubmissionId(string submissionId)
        {
            var submission = this.context.Submissions.Where(x => x.Id == submissionId).SingleOrDefault();
            return submission.ProblemId;
        }
    }
}
