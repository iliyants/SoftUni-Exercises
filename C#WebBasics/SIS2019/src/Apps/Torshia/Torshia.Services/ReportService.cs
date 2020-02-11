
using System;
using System.Linq;
using Torshia.Data;
using Torshia.Models;
using Torshia.Models.Enums;

namespace Torshia.Services
{
    public class ReportService : IReportService
    {
        private readonly ToshiaDbContext context;

        public ReportService(ToshiaDbContext context)
        {
            this.context = context;
        }
        public void CreateReport(string userId, string taskId)
        {
            var report = new Report()
            {
                ReportedOn = DateTime.UtcNow,
                Status = ReportIsCompletedOnRandom(),
                UserId = userId,
                TaskId = taskId
            };

            this.context.Reports.Add(report);
            this.context.SaveChanges();
        }

        public string GetReportDateByTaskId(string taskId)
        {
            return this.context.Reports
                .Where(x => x.TaskId == taskId)
                .Select(x => x.ReportedOn.ToShortDateString())
                .SingleOrDefault().ToString();
        }

        public string GetReporterNameByTaskId(string taskId)
        {
            return this.context.Reports
                .Where(x => x.TaskId == taskId)
                .Select(x => x.Reporter.Username)
                .SingleOrDefault().ToString();
        }

        public string GetReportIdByTaskId(string taskId)
        {
            return this.context.Reports
                .Where(x => x.TaskId == taskId)
                .Select(x => x.Id)
                .SingleOrDefault().ToString();
        }

        public string GetReportSatusByTask(string taskId)
        {
            var report = this.context.Reports
                .Where(x => x.Task.Id == taskId)
                .Select(x => x.Status.ToString())
                .FirstOrDefault();

            return report;
        }

        private StatusType ReportIsCompletedOnRandom()
        {
            Random r = new Random();
            var result =  r.Next(0, 100);

            if (result <= 75)
            {
                return StatusType.Completed;
            }

            return StatusType.Archived;
        }
    }
}
