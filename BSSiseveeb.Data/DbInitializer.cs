using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using BSSiseveeb.Core;
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

            _ctx.Employees.Add(new Employee
            {
                Name = "Toomas Käär",
                Birthdate = new DateTime(1993, 9, 1),
                ContractStart = new DateTime(2016, 2, 14),
                ContractEnd = new DateTime(2016, 5, 14),
                PhoneNumber = "+37253489161",
                VacationDays = 14
            });

            _ctx.Employees.Add(new Employee()
            {
                Name = "Testija kaks",
                Birthdate = new DateTime(1993, 7, 25),
                ContractStart = new DateTime(2016, 2, 14),
                ContractEnd = new DateTime(2016, 5, 14),
                PhoneNumber = "+37253489161",
                VacationDays = 14
            });

            var adminRole = new Role
            {
                Name = "Administrator",
                Rights = AccessRights.All,
            };

            var user = new ApplicationUser()
            {
                EmployeeId = 1,
                PasswordHash = _hasher.HashPassword("Password1"),
                RoleId = 1,
                Messages = "no",
                Email = "test@test.ee",
                EmailConfirmed = true,
                LockoutEnabled = false,
                UserName = "test@test.ee",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            
            adminRole.Users.Add(new IdentityUserRole() {RoleId = adminRole.Id, UserId = user.Id});


            _ctx.Roles.Add(adminRole);
            _ctx.Users.Add(user);

            _ctx.Vacations.Add(new Vacation()
            {
                Days = 11,
                EmployeeId = 1,
                EndDate = new DateTime(2016, 4, 10),
                StartDate = new DateTime(2016, 4, 5),
                Status = VacationStatus.Approved
            });

            _ctx.Vacations.Add(new Vacation()
            {
                Days = 10,
                EmployeeId = 2,
                EndDate = new DateTime(2016, 4, 4),
                StartDate = new DateTime(2016, 3, 18),
                Status = VacationStatus.Approved
            });

            _ctx.Vacations.Add(new Vacation()
            {
                Days = 5000,
                EmployeeId = 1,
                EndDate = new DateTime(2016, 2, 5),
                StartDate = new DateTime(2016, 1, 4),
                Status = VacationStatus.Approved
            });



            base.Seed(context);
        }

    }
}
