using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BSSiseveeb.Private.Web.Scheduled_Tasks.Jobs;
using Quartz;
using Quartz.Impl;

namespace BSSiseveeb.Private.Web.Scheduled_Tasks
{
    public class Scheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail monthlyBirthdayJob = JobBuilder.Create<MonthlyBirthdayJob>().Build();
            ITrigger monthlyTrigger = TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(6, 10, 5))
                .Build();

            scheduler.ScheduleJob(monthlyBirthdayJob, monthlyTrigger);
        }
    }
}