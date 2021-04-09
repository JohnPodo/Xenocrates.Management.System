using System.Linq;
using ManagementSystemVersionTwo.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManagementSystemVersionTwo.Startup))]
namespace ManagementSystemVersionTwo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            CleanDatabaseFromWrongEntries();
        }

        /// <summary>
        /// Finds ApplicationUsers with no Worker and no Role and Delete them
        /// </summary>
        private void CleanDatabaseFromWrongEntries()
        {
            using(ApplicationDbContext context =new ApplicationDbContext())
            {
                var wrongUsers = context.Users.Where(u => u.Worker == null && u.Roles.Count == 0).ToList();
                foreach(var user in wrongUsers)
                {
                    var userInDb = context.Users.Find(user.Id);
                    context.Users.Remove(userInDb);
                }
                context.SaveChanges();
            }
        }
    }
}
