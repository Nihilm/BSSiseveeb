using System;
using System.Data.Entity;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Contracts.Repositories;

namespace BSSiseveeb.Data
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<BSContext>
    {
        private BSContext _ctx;

        protected override void Seed(BSContext context)
        {
            _ctx = context;

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

            _ctx.Roles.Add(adminRole);
            _ctx.Roles.Add(userRole);
            _ctx.Roles.Add(requestHandler);
            _ctx.Roles.Add(vacationHandler);
            _ctx.Roles.Add(userHandler);

            

            base.Seed(context);
        }

    }
}
