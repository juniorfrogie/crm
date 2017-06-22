
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Specialized;

namespace CRM
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            // have scheduler factory and scheduler in static fields that they don't get garbage collected
            NameValueCollection props = new NameValueCollection();
            props["quartz.threadPool.makeThreadsDaemons"] = "true";
            props["quartz.scheduler.makeSchedulerThreadDaemon"] = "true";
            StdSchedulerFactory sessionFactory = new StdSchedulerFactory(props);
            scheduler = sessionFactory.GetScheduler();
            scheduler.Start();
            
            IJobDetail job = JobBuilder.Create<EmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule
                  (s =>
                     s.WithIntervalInHours(8)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                  )
                .WithIdentity("sendEmail", "CRM")
                .StartNow()
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }

}