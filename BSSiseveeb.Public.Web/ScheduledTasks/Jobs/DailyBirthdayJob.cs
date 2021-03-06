﻿using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Contracts.Services;
using BSSiseveeb.Core.Domain;
using Quartz;
using System;
using System.Linq;

namespace BSSiseveeb.Public.Web.ScheduledTasks.Jobs
{
    public class DailyBirthdayJob : IJob
    {
        private readonly IEmployeeRepository EmployeeRepository;
        public IEmailService EmailService { get; set; }

        public DailyBirthdayJob()
        {
            EmployeeRepository = IoC.Resolve<IEmployeeRepository>();
        }

        public void Execute(IJobExecutionContext context)
        {
            var employees = EmployeeRepository.Where(x => x.DailyBirthdayMessages || x.Birthdate.Value.Day == DateTime.Now.Day)
                .Select(x => new Employee
                {
                    Email = x.Email,
                    Birthdate = x.Birthdate,
                    Name = x.Name,
                    DailyBirthdayMessages = x.DailyBirthdayMessages
                });

            var emails = employees.Where(x => x.DailyBirthdayMessages).Select(x => x.Email);
            var birthdays = employees.Where(x => x.Birthdate.Value.Day == DateTime.Now.Day).ToList();

            if (birthdays.Count == 0)
            {
                return;
            }

            EmailService.TodayBirthday(birthdays, emails);
        }
    }
}