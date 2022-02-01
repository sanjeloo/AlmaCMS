using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AlmaCMS.Jobs
{
    public class ExecuteTaskServiceCallJob : IJob

    {

       
        public static readonly string SchedulingStatus = ConfigurationManager.AppSettings["ExecuteTaskServiceCallSchedulingStatus"];
        public void Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() =>
            {
                if (SchedulingStatus.Equals("ON"))
                {
                    try
                    {
                        //Do whatever stuff you want

                        Helpers.SMSNotifications.SendDailySMSAlarm();
                        Helpers.SMSNotifications.SendBirtDateGift();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
            //return task;
        }

        
    }
}