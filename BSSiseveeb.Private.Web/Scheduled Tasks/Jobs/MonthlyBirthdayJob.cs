using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Data.Repositories;
using BSSiseveeb.Private.Web.Controllers.API.Helpers;
using Microsoft.AspNet.Identity.Owin;
using Quartz;

namespace BSSiseveeb.Private.Web.Scheduled_Tasks.Jobs
{
    public class MonthlyBirthdayJob : IJob
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        public void Execute(IJobExecutionContext context)
        {
            var emails = EmployeeRepository.Where(x => x.Account.Messages == "Yes").AsDto().Select(x => x.Email);
            var birthdays = EmployeeRepository.Where(x => x.Birthdate.Month == DateTime.Now.Month).AsDto();
            var subject = "Selle kuu sünnipäevalised!";
            var body = $"<ul><li>Sellel kuul on sünnipäev:</li>";

            foreach (var birthday in birthdays)
            {
                body += $"<li>{birthday.Name}: {birthday.Birthdate.ToString("d")}</li>";
            }
            body += "</ul></br><h3>Soovime neile kõigile head sünnipäeva!</h3>";

            EmailHelper.SendEmail(emails, subject, body);
        }
    }
}