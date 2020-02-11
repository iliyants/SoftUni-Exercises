using SULS.Models;
using System.Collections.Generic;
using System.Linq;

namespace SULS.Services
{
    public interface IProblemService
    {
        void CreateProblem(string name, int points);

        bool CheckForDuplicateProblemNames(string name);

        IQueryable<Problem> GetAllProblems();

        Problem GetProblemById(string id);

        void IncrementProblemSubmissions(string problemId);

        void DecrementPoblemSubmissions(string problemId);

    }
}
