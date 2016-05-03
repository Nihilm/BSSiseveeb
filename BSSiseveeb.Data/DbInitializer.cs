using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using BSSiseveeb.Core.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BSSiseveeb.Data
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<BSContext>
    {
        private BSContext _ctx;
        private PasswordHasher _hasher;

        protected override void Seed(BSContext context)
        {
            _ctx = context;
            _hasher = new PasswordHasher();

            var emp1 = new Employee
            {
                Name = "Toomas Käär",
                Birthdate = new DateTime(1993, 9, 1),
                ContractStart = new DateTime(2016, 2, 14),
                ContractEnd = new DateTime(2016, 5, 14),
                PhoneNumber = "+37253489161",
                VacationDays = 28,
                Email = "test@test.ee"
            };

            var emp2 = new Employee()
            {
                Name = "Testija kaks",
                Birthdate = new DateTime(1993, 4, 25),
                ContractStart = new DateTime(2016, 2, 14),
                ContractEnd = new DateTime(2016, 5, 14),
                PhoneNumber = "+37253489161",
                VacationDays = 28,
                Email = "tester@test.ee"
            };

            var emp3 = new Employee()
            {
                Name = "Testija kolm",
                Birthdate = new DateTime(1993, 4, 25),
                ContractStart = new DateTime(2016, 2, 14),
                ContractEnd = new DateTime(2016, 5, 14),
                PhoneNumber = "+37253489161",
                VacationDays = 28,
                Email = "tester3@test.ee"
            };

            var adminRole = new Role
            {
                Name = "Administrator",
                Rights = AccessRights.All,
            };

            var userRole = new Role
            {
                Name = "User",
                Rights = AccessRights.Level1
            };

            var requestHandler = new Role()
            {
                Name = "RequestHandler",
                Rights = AccessRights.Level1 | AccessRights.Level2 | AccessRights.Level3 | AccessRights.Level4 
            };

            var user = new ApplicationUser()
            {
                EmployeeId = 1,
                PasswordHash = _hasher.HashPassword("Password1"),
                RoleId = adminRole.Id,
                Messages = "no",
                Email = "test@test.ee",
                EmailConfirmed = true,
                LockoutEnabled = false,
                UserName = "test@test.ee",
                SecurityStamp = Guid.NewGuid().ToString(),
                Employee = emp1
            };

            var user2 = new ApplicationUser()
            {
                EmployeeId = 2,
                PasswordHash = _hasher.HashPassword("Password1"),
                RoleId = userRole.Id,
                Messages = "no",
                Email = "tester@test.ee",
                EmailConfirmed = true,
                LockoutEnabled = false,
                UserName = "tester@test.ee",
                SecurityStamp = Guid.NewGuid().ToString(),
                Employee = emp2
            };

            var user3 = new ApplicationUser()
            {
                EmployeeId = 3,
                PasswordHash = _hasher.HashPassword("Password1"),
                RoleId = requestHandler.Id,
                Messages = "no",
                Email = "tester3@test.ee",
                EmailConfirmed = true,
                LockoutEnabled = false,
                UserName = "tester3@test.ee",
                SecurityStamp = Guid.NewGuid().ToString(),
                Employee  = emp3
            };

            adminRole.Users.Add(new IdentityUserRole() {RoleId = adminRole.Id, UserId = user.Id});
            userRole.Users.Add(new IdentityUserRole() {RoleId = userRole.Id, UserId = user2.Id});
            requestHandler.Users.Add(new IdentityUserRole() {RoleId = requestHandler.Id, UserId = user3.Id});

            _ctx.Roles.Add(adminRole);
            _ctx.Roles.Add(userRole);
            _ctx.Roles.Add(requestHandler);
            _ctx.Users.Add(user);
            _ctx.Users.Add(user2);
            _ctx.Users.Add(user3);

            base.Seed(context);
        }

    }
}
