using DoorPanes.Services.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoorPanes.WebApp.Startup))]
namespace DoorPanes.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
        }

        /// <summary>
        /// Method to create roles.
        /// </summary>
        private void CreateRoles()
        {
            // create login context for role creation
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // create staff role
            if (!roleManager.RoleExists("Staff"))
            {
                roleManager.Create(new IdentityRole { Name = "Staff" });
            }

            // create faculty role
            if (!roleManager.RoleExists("Faculty"))
            {
                roleManager.Create(new IdentityRole { Name = "Faculty" });
            }

            // create student role
            if (!roleManager.RoleExists("Student"))
            {
                roleManager.Create(new IdentityRole { Name = "Student" });
            }
        }
    }
}
