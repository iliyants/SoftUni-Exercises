using SULS.Data;
using SULS.Models;
using System.Linq;

namespace SULS.Services
{
    public class ProblemService : IProblemService
    {

        private readonly SULSDbContext context;

        public ProblemService(SULSDbContext context)
        {
            this.context = context;
        }
        public bool CheckForDuplicateProblemNames(string name)
        {
            return this.context.Problems.Any(x => x.Name == name);
        }

        public void CreateProblem(string name, int points)
        {
            var problem = new Problem()
            {
                Name = name,
                Points = points
            };

            this.context.Problems.Add(problem);
            this.context.SaveChanges();
        }

        public IQueryable<Problem> GetAllProblems()
        {
            return this.context.Problems;
        }

        public Problem GetProblemById(string id)
        {
            return this.context.Problems.SingleOrDefault(x => x.Id == id);
        }

        public void IncrementProblemSubmissions(string problemId)
        {
            var problem  = this.context.Problems.SingleOrDefault(x => x.Id == problemId);
            problem.Submissions++;
            context.Update(problem);
            context.SaveChanges();
        }

        public void DecrementPoblemSubmissions(string problemId)
        {
            var problem = this.context.Problems.SingleOrDefault(x => x.Id == problemId);
            problem.Submissions--;
            context.Update(problem);
            context.SaveChanges();
        }

    }
}
