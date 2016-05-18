using BSSiseveeb.Core.Domain;
using BSSiseveeb.Data.Migrations;
using Sparkling.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

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
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BSContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Employee>().Property(x => x.Email).HasMaxLength(128);
            modelBuilder.Entity<Employee>().Property(x => x.Name).HasMaxLength(128);
            modelBuilder.Entity<Employee>().Property(x => x.PhoneNumber).HasMaxLength(32);
            modelBuilder.Entity<Employee>().Property(x => x.Skype).HasMaxLength(64);
            modelBuilder.Entity<Request>().Property(x => x.Req).HasMaxLength(128);
            modelBuilder.Entity<Request>().Property(x => x.Description).HasMaxLength(512);
            modelBuilder.Entity<Role>().Property(x => x.Name).HasMaxLength(32);
            modelBuilder.Entity<Vacation>().Property(x => x.Comments).HasMaxLength(512);

            
        }
    }
}