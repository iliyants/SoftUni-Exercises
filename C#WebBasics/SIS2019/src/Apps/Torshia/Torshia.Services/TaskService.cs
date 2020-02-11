using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Torshia.Data;
using Torshia.Models;
using Torshia.Services.DTOs;

namespace Torshia.Services
{
    public class TaskService : ITaskService
    {

        private readonly ToshiaDbContext context;
        private readonly IUserService userService;
        private readonly ISectorService sectorService;

        public TaskService(ToshiaDbContext context, IUserService userService, ISectorService sectorService)
        {
            this.context = context;
            this.userService = userService;
            this.sectorService = sectorService;
        }

        public void CreateTask
            (string title, string dueDate, string description, string participants, List<string> affectedSectors)
        {

            var task = new Task()
            {
                Title = title,
                DueDate = dueDate ?? "None",
                Description = description,
            };

            foreach (var sector in affectedSectors)
            {
                var affectedSector = new TasksSectors()
                {
                    SectorId = this.sectorService.GetSectorByName(sector).Id,
                    TaskId = task.Id
                };

                task.AffectedSectors.Add(affectedSector);
            }

            var users = this.userService.ReturnUsersByUsernames(participants);

            foreach (var user in users)
            {
                var taskUser = new UsersTasks()
                {
                    TaskId = task.Id,
                    UserId = user.Id
                };

                task.TaskUsers.Add(taskUser);
            }

            this.context.Tasks.Add(task);
            this.context.SaveChanges();
        }


        public IEnumerable<Task> GetAllTasks()
        {

            return this.context.Tasks
                .Include(x => x.AffectedSectors)
                .Include(x => x.TaskUsers)
                .ToList();
        }

        public TaskDTO GetTaskById(string id)
        {
            var task = this.context
                .Tasks
                .Where(x => x.Id == id)
                .Include(x => x.TaskUsers)
                .Include(x => x.AffectedSectors)
                .SingleOrDefault();

            return this.context.Tasks.Where(x => x.Id == id)
                .Select(x => new TaskDTO()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Level = x.AffectedSectors.Count(),
                    Participants = x.TaskUsers.Select(user => user.User.Username).ToList(),
                    DueDate = x.DueDate,
                    AffectedSectors = x.AffectedSectors.Select(sector => sector.Sector.Name).ToList(),
                    Description = x.Description
                }).SingleOrDefault();
        }


        public void ReportATask(string taskId)
        {
            var task = this.context.Tasks.SingleOrDefault(x => x.Id == taskId);
            task.IsReported = true;
            this.context.Update(task);
            context.SaveChanges();
        }
    }
}
