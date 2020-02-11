using SULS.Models;
using System.Linq;

namespace SULS.Services
{
    public interface ISubmissionService
    {
        void CreateSubmission(string code, int problemMaxPoints, string problemId, string userId);

        IQueryable<Submission> GetAllSubmissionsByProblem(string problemId);

        void DeleteSubmission(string submissionId);

        string GetProblemIdBySubmissionId(string submissionId);

    }
}
