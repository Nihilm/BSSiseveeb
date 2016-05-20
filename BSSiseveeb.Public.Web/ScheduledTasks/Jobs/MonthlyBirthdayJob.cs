using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Contracts.Services;
using BSSiseveeb.Core.Domain;
using Quartz;
using System;
using System.Linq;

namespace BSSiseveeb.Public.Web.ScheduledTasks.Jobs
{
    public class MonthlyBirthdayJob : IJob
    {
        private readonly IEmployeeRepository EmployeeRepository;
        public IEmailService EmailService { get; set; }

        public MonthlyBirthdayJob()
        {
            EmployeeRepository = IoC.Resolve<IEmployeeRepository>();
        }

        public void Execute(IJobExecutionContext context)
        {
            var employees = EmployeeRepository.Where(x => x.MonthlyBirthdayMessages || x.Birthdate.Value.Month == DateTime.Now.Month)
                .Select(x => new Employee
                {
                    Email = x.Email,
                    Birthdate = x.Birthdate,
                    Name = x.Name,
                    MonthlyBirthdayMessages = x.MonthlyBirthdayMessages
                });

            var emails = employees.Where(x => x.MonthlyBirthdayMessages).Select(x => x.Email);
            var birthdays = employees.Where(x => x.Birthdate.Value.Month == DateTime.Now.Month).ToList();

            if (birthdays.Count == 0)
            {
                return;
            }

            EmailService.MonthBirthday(birthdays, emails);
        }
    }
}

