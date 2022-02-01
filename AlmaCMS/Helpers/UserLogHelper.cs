using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using System.Web.Security;

namespace AlmaCMS.Helpers
{
    public class UserLogHelper
    {
        public static DB_AlmaCmsEntities db;
        public static UserLogRepository repUserLog;

        public UserLogHelper()
        {
            db=new DB_AlmaCmsEntities();
            repUserLog = new UserLogRepository(db);
         }


        public static int GetUserId(string useremail)
        {
            db = new DB_AlmaCmsEntities();
            var currentUSer = db.Admins.Where(c => c.Email == useremail).FirstOrDefault();
            if (currentUSer != null)
            {
                return currentUSer.Id;
            }
            else
           
                return 0;
            }
        public static bool AddLog(string PageTitle,String UserName,string Action,string PostTitle)
        {
            db = new DB_AlmaCmsEntities();
            repUserLog = new UserLogRepository(db);
            try
            {
                UserLog NewLog = new UserLog()
                {
                    Action = Action,
                    UserId  = GetUserId(UserName),
                    dateinsert = DateTime.Now,
                    PageTitle = PageTitle,
                    PostTitle = PostTitle,
                    UserName = UserName

                };
                repUserLog.Insert(NewLog);
                return true;
        }
            catch(Exception)
            {
                return false;
                throw;
            }

        }
        
    }
}