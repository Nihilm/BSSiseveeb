using System.Data.Entity;
using BSSiseveeb.Core.Domain;
using Sparkling.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSSiseveeb.Data
{
    public interface IBSContextContextManager : IDbContextManager<BSContext>
    {
    }

    public class BSContextDbContextManager : DbContextManager<BSContext>, IBSContextContextManager
    {
    }

    public class BSContext : DbContext
    {
        public DbSet<UserTokenCache> UserTokenCacheList { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Role> Roles { get; set; }

        public BSContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new DbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}