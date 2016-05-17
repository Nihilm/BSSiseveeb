using BSSiseveeb.Public.Web.ScheduledTasks.Jobs;
using Quartz;
using Quartz.Impl;

namespace BSSiseveeb.Public.Web.ScheduledTasks
{
    public class Scheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail monthlyBirthdayJob = JobBuilder.Create<MonthlyBirthdayJob>().Build();
            IJobDetail dailyBirthdayJob = JobBuilder.Create<DailyBirthdayJob>().Build();
            IJobDetail dailyVacationJob = JobBuilder.Create<RetireVacationsJob>().Build();

            ITrigger monthlyBirthdayTrigger = TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 8, 0))
                .Build();
            ITrigger dailyVacationTrigger = TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(2, 0))
                .Build();
            ITrigger dailyBirthdayTrigger = TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(2, 0))
                .Build();

            scheduler.ScheduleJob(monthlyBirthdayJob, monthlyBirthdayTrigger);
            scheduler.ScheduleJob(dailyBirthdayJob, dailyBirthdayTrigger);
            scheduler.ScheduleJob(dailyVacationJob, dailyVacationTrigger);
        }
    }
}