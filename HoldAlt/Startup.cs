using HoldAlt.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using HoldAlt.Classes;
using HoldAlt.Models;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(HoldAlt.Startup))]
namespace HoldAlt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            loadAltCoins();

        }

        // Create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            /*

            ApplicationDbContext context = new ApplicationDbContext();

            
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                string userPWD = "N@rain00";

                user = UserManager.FindByEmail(userPWD);
                if (user == null)
                {
                    var user1 = new ApplicationUser();
                    user1.UserName = "admin@admin.com";
                    user1.Email = "admin@admin.com";

                    var chkUser = UserManager.Create(user1, userPWD);

                    //Add default User to Role Admin   
                    if (chkUser.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user1.Id, "Admin");

                    }
                }
                else
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating QMS Subscriber role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }


            /**/


        }

        private void loadAltCoins()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Globals.lAltCoins = db.AltCoins.ToList();
        }
    }
}
