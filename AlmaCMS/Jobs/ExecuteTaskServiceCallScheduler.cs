using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Quartz.Impl;

using System.Configuration;

using Quartz;
namespace AlmaCMS.Jobs
{
    public class ExecuteTaskServiceCallScheduler
    {
        private static readonly string ScheduleCronExpression = ConfigurationManager.AppSettings["ExecuteTaskScheduleCronExpression"];
        public static async System.Threading.Tasks.Task StartAsync()
        {
            try
            {
                var scheduler =  StdSchedulerFactory.GetDefaultScheduler();
                if (!scheduler.IsStarted)
                {
                     scheduler.Start();
                }
                var job = JobBuilder.Create<ExecuteTaskServiceCallJob>().Build();
                ITrigger trigger = TriggerBuilder.Create()
       .WithIdentity("trigger1", "group1")
       .StartNow()
       .WithDailyTimeIntervalSchedule(p => p.StartingDailyAt(new TimeOfDay(0, 0, 0)).WithIntervalInMinutes(5))

       .Build();

                 scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {

            }
        }
    }
}