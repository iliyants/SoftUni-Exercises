using SIS.MvcFramework;
using SIS.MvcFramework.Routing;
using SULS.Data;
using SULS.Services;
using System;


namespace SULS.App
{
    public class Startup : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (var context = new SULSDbContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public void ConfigureServices(SIS.MvcFramework.DependencyContainer.IServiceProvider serviceProvider)
        {
            serviceProvider.Add<IUserService, UserService>();
            serviceProvider.Add<ISubmissionService, SubmissionService>();
            serviceProvider.Add<IProblemService, ProblemService>();
        }
    }
}
