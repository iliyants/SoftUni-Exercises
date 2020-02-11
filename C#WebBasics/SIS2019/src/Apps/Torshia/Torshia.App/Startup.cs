using SIS.MvcFramework;
using SIS.MvcFramework.DependencyContainer;
using SIS.MvcFramework.Routing;
using Torshia.Data;
using Torshia.Services;

namespace Torshia.App
{
    public class Startup : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (var db = new ToshiaDbContext())
            {
                //db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

        }

        public void ConfigureServices(IServiceProvider serviceProvider)
        {
            serviceProvider.Add<IUserService, UserService>();
            serviceProvider.Add<ITaskService, TaskService>();
            serviceProvider.Add<IReportService, ReportService>();
            serviceProvider.Add<ISectorService, SectorService>();
        }
    }
}
