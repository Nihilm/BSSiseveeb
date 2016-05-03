using System.Data.Entity;
using BSSiseveeb.Core.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using Sparkling.Data;

namespace BSSiseveeb.Data
{
    public interface IBSContextContextManager : IDbContextManager<BSContext>
    {
    }

    public class BSContextDbContextManager : DbContextManager<BSContext>, IBSContextContextManager
    {
    }

    public class BSContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vacation> Vacations { get; set; }

        public BSContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new DbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(x => x.Employee)
                .WithRequired(x => x.Account);
        }
    }

    
}