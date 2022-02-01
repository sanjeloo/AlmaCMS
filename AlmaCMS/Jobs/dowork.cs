using AlmaCMS.Helpers;
using AlmaCMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AlmaCMS.Jobs
{
    public static class dowork
    {
        public static void write()
        {
            //string fileName = @"E:\myfile.txt";
            //using (StreamWriter sw = System.IO.File.AppendText(fileName))
            //{
            //    sw.WriteLine(DateTime.Now);
              
            //}


            //return;

            try
            {


                var db = new DB_AlmaCmsEntities();
                var TaskList = db.TasksLists.Where(c => c.SendSMSAlarm == true && c.StatusId < 3).ToList();
                foreach (var item in TaskList)
                {
                    var taskusers = db.TaskExperts.Where(c => c.TaskId == item.id).ToList();
                    var Currentadmin = db.AspNetUsers.Find(item.AdminId);
                    if (item.ExpireDate > DateTime.Now)
                    {
                        if (db.SendSMS.Where(c => c.TaskId == item.id && c.IsNotEnded == true).Count() == 0)
                        {

                            foreach (var itemexpert in taskusers)
                            {
                                var CurrenExpert = db.AspNetUsers.Find(itemexpert.UseId);
                                SMSHelper.SendSMS(CurrenExpert.UserName, "", "یک وطیفه در مورد مقرر انجام نشد , مهلت انجام به پایان رسید.عنوان وظیفه : " + item.Title);
                                SMSHelper.SendSMS(Currentadmin.UserName, "", "یک وطیفه در مورد مقرر انجام نشد , مهلت انجام به پایان رسید.عنوان وظیفه : " + item.Title);


                            }

                            db.SendSMS.Add(new SendSM() { DateInsert = DateTime.Now, IsNotEnded = true, TaskId = item.id });
                            db.SaveChanges();
                        }

                    }

                    else
                    {
                        var diffdate = item.ExpireDate - DateTime.Now;
                        if (diffdate.Value.Days < 30)
                        {
                            var LAstsms = db.SendSMS.Where(c => c.TaskId == item.id);
                            DateTime lastSMSDate = item.ExpireDate.Value;
                            if (LAstsms.Count() > 0)
                            {
                                lastSMSDate = LAstsms.OrderByDescending(c => c.id).FirstOrDefault().DateInsert.Value;

                                if (diffdate.Value.Days > 7)
                                {
                                    var difflast = DateTime.Now - lastSMSDate;
                                    if (difflast.Days > 6 && DateTime.Now.Hour < 21 && difflast.Hours > 7)
                                    {
                                        foreach (var itemexpert in taskusers)
                                        {
                                            var CurrenExpert = db.AspNetUsers.Find(itemexpert.UseId);
                                            SMSHelper.SendSMS(CurrenExpert.UserName, "", "یک وطیفه برای انجام دارید : " + item.Title);

                                            db.SendSMS.Add(new SendSM() { DateInsert = DateTime.Now, IsNotEnded = false, TaskId = item.id });
                                            db.SaveChanges();
                                        }
                                    }


                                    if (diffdate.Value.Days < 7 && diffdate.Value.Days > 2)
                                    {
                                        var difflastdays = DateTime.Now - lastSMSDate;
                                        if (difflast.Hours > 24 && DateTime.Now.Hour < 21 && difflast.Hours > 7)
                                        {
                                            foreach (var itemexpert in taskusers)
                                            {
                                                var CurrenExpert = db.AspNetUsers.Find(itemexpert.UseId);
                                                SMSHelper.SendSMS(CurrenExpert.UserName, "", "یک وطیفه برای انجام دارید : " + item.Title);

                                                db.SendSMS.Add(new SendSM() { DateInsert = DateTime.Now, IsNotEnded = false, TaskId = item.id });
                                                db.SaveChanges();
                                            }
                                        }
                                    }

                                    if (diffdate.Value.Days < 2)
                                    {
                                        var difflastdays = DateTime.Now - lastSMSDate;
                                        if (difflast.Hours > 2 && DateTime.Now.Hour < 2 && difflast.Hours > 7)
                                        {
                                            foreach (var itemexpert in taskusers)
                                            {
                                                var CurrenExpert = db.AspNetUsers.Find(itemexpert.UseId);
                                                SMSHelper.SendSMS(CurrenExpert.UserName, "", "یک وطیفه برای انجام دارید : " + item.Title);

                                                db.SendSMS.Add(new SendSM() { DateInsert = DateTime.Now, IsNotEnded = false, TaskId = item.id });
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                   
                }

                return;
            }
            catch (Exception)
            {

                throw;
                return;
            }

        }
        

    }
}


                   
            