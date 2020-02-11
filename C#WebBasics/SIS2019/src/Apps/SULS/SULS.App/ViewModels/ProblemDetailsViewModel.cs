using System;

namespace SULS.App.ViewModels
{
    public class ProblemDetailsViewModel
    {
        public string Username { get; set; }

        public DateTime CreatedOn { get; set; }

        public int AchievedResult { get; set; }

        public int MaxPoints { get; set; }

        public string SubmissionId { get; set; }


    }
}
