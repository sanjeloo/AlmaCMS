using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.Helpers
{
    public class SMSHelper
    {

        public static bool SendSMS(string ToNumber, string Subject, string MessageText)
        {


            try
            {
                string SMSUser = System.Web.Configuration.WebConfigurationManager.AppSettings["SMSUser"].ToString();
                string SMSPass = System.Web.Configuration.WebConfigurationManager.AppSettings["SMSPass"].ToString();


                ir.smsran.SendSMS sms = new ir.smsran.SendSMS();
                sms.Send_Sms5(SMSUser, SMSPass, ToNumber, "210002545", MessageText, 6, true, 1, DateTime.Now, false);
                return true;
         }
            catch (Exception ex)
            {

                return false;
            }


        }
    }
}