using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Helpers;
using AlmaCMS.Repositories;
using System.Net;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace AlmaCMS.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersMessagesController : Controller
    { DB_AlmaCmsEntities db;
        UserInfoRepositiry repUserInfo;

        //UserMessageRepository RepMessages;
        MessageAnswerRepositoy repAnswers;
        public ManageUsersMessagesController()
        {
            db = new DB_AlmaCmsEntities();
            repUserInfo = new UserInfoRepositiry(db);
            //RepMessages = new UserMessageRepository(db);
            repAnswers = new MessageAnswerRepositoy(db);

        }
        // GET: Manage/ManageUsersMessages

        public ManageUsersMessagesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: Manage/ManageTasks
        public ActionResult Index()
        {
          

        





            return View();
        }
    }
}