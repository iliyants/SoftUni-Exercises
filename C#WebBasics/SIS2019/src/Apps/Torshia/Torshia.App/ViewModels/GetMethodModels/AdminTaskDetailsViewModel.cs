namespace Torshia.App.ViewModels.GetMethodModels
{
    public class AdminTaskDetailsViewModel
    {
        public string ReportId { get; set; }

        public string TaskName { get; set; }

        public int Level { get; set; }
        public string TaskStatus { get; set; }

        public string DueDate { get; set; }
        public string ReportedOn { get; set; }

        public string Reporter { get; set; }

        public string Participants { get; set; }

        public string AffectedSectors { get; set; }

        public string TaskDescription { get; set; }
    }
}
