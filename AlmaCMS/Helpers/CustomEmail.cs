using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace AlmaCMS.Helpers
{
    public class CustomEmail
    {
        public static bool sendemail(string subject, string toEmail, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(System.Web.Configuration.WebConfigurationManager.AppSettings["SMTPEmail"].ToString());
            mail.From = new MailAddress(System.Web.Configuration.WebConfigurationManager.AppSettings["EmailFrom"].ToString(), "پی یو صنعت نظری");
            //foreach (var item in toEmails)
            //{
            //    mail.To.Add(item);
            //}
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = int.Parse(System.Web.Configuration.WebConfigurationManager.AppSettings["Port"].ToString());
            SmtpServer.Credentials = new System.Net.NetworkCredential(System.Web.Configuration.WebConfigurationManager.AppSettings["EmailUserName"].ToString(),
                                       System.Web.Configuration.WebConfigurationManager.AppSettings["EmailPass"].ToString());
            SmtpServer.EnableSsl = false;
            try
            {
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string sendemail(string subject, string toEmails, string body, System.Net.Mail.Attachment attach)
        {



            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPEmail"].ToString());

            mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailFrom"].ToString());

            mail.To.Add(toEmails);
            mail.Attachments.Add(attach);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Port"].ToString());
            SmtpServer.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["EmailFrom"].ToString(), System.Configuration.ConfigurationManager.AppSettings["EmailPass"].ToString());//mail,pass
            SmtpServer.EnableSsl = false;
            try
            {
                SmtpServer.Send(mail);
                return "Ok";
            }
            catch (Exception ex)
            {
                return "false";
            }


        }

    }
}