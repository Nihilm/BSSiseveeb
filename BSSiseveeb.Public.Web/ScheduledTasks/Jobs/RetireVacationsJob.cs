using BSSiseveeb.Core;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Public.Web.Controllers.API;
using EntityFramework.Extensions;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSSiseveeb.Public.Web.ScheduledTasks.Jobs
{
    public class RetireVacationsJob : IJob
    {
        private readonly IVacationRepository VacationRepository;

        public RetireVacationsJob()
        {
            VacationRepository = IoC.Resolve<IVacationRepository>();
        }

        public void Execute(IJobExecutionContext context)
        {
            VacationRepository
                .Where(x => x.EndDate < DateTime.Now && x.Status == VacationStatus.Approved)
                .Update(x => new Vacation()
                {
                    Status = VacationStatus.Retired
                });
        }
    }
}