using BSSiseveeb.Core.Domain;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BSSiseveeb.Data.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<BSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            ContextKey = "BSSiseveeb.Data.BSContext";
        }

        protected override void Seed(BSContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            var adminRole = new Role
            {
                Name = "Administrator",
                Rights = AccessRights.Administrator,
            };

            var userRole = new Role
            {
                Name = "User",
                Rights = AccessRights.Standard
            };

            var requestHandler = new Role
            {
                Name = "RequestHandler",
                Rights = AccessRights.Standard | AccessRights.Requests
            };

            var vacationHandler = new Role
            {
                Name = "VacationHandler",
                Rights = AccessRights.Standard | AccessRights.Vacations
            };

            var userHandler = new Role
            {
                Name = "UserHandler",
                Rights = AccessRights.Standard | AccessRights.Users
            };

            context.Roles.Add(adminRole);
            context.Roles.Add(userRole);
            context.Roles.Add(requestHandler);
            context.Roles.Add(vacationHandler);
            context.Roles.Add(userHandler);

            base.Seed(context);
        }
    }
}