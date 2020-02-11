using Torshia.Models;

namespace Torshia.Services
{
    public interface IReportService
    {
        void CreateReport(string userId, string taskId);

        string GetReportSatusByTask(string taskId);

        string GetReportIdByTaskId(string taskId);

        string GetReportDateByTaskId(string taskId);

        string GetReporterNameByTaskId(string taskId);
    }
}
