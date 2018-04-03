using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HoldAlt.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<HoldAlt.Models.AltCoin> AltCoins { get; set; }

        public System.Data.Entity.DbSet<HoldAlt.Models.Exchange> Exchanges { get; set; }

        public System.Data.Entity.DbSet<HoldAlt.Models.Holding> Holdings { get; set; }

        public System.Data.Entity.DbSet<HoldAlt.Models.Trade> Trades { get; set; }

        public System.Data.Entity.DbSet<HoldAlt.Models.StockTrade> StockTrades { get; set; }

        public System.Data.Entity.DbSet<HoldAlt.Models.StockHolding> StockHoldings { get; set; }

    }
}