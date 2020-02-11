using System.Collections.Generic;
using System.Linq;
using Torshia.Models;
using Torshia.Services.DTOs;

namespace Torshia.Services
{
    public interface ITaskService
    {
        void CreateTask(string title, string dueDate, string description,string participants, List<string> affectedSectors);

        IEnumerable<Task> GetAllTasks();

        void ReportATask(string taskId);

        TaskDTO GetTaskById(string id);


    }
}
