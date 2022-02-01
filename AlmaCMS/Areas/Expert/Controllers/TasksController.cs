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

namespace AlmaCMS.Areas.Expert.Controllers
{

    [Authorize(Roles="Expert")]
    public class TasksController : Controller
    {

        DB_AlmaCmsEntities db;
        TasksRepository repTask;
        UserInfoRepositiry repUserInfo;
        TaskAnswerRepository repAnswers;
        public TasksController()
        {
            db = new DB_AlmaCmsEntities();
            repTask = new TasksRepository(db);
            repUserInfo = new UserInfoRepositiry(db);
            
            repAnswers = new TaskAnswerRepository(db);
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
        // GET: Expert/Taks
        public ActionResult Index()
        {

            var Userif = User.Identity.GetUserId();
            var TasksList = repTask.GetDapperExpertTaskList().Where(c => c.UseId == Userif).Where(c=>c.StatusId!=4).ToList();

            return View(TasksList);
        }

        public ActionResult Details(int id)
        {
            var CurrentTask = repTask.GetDapperTaskList().Where(c => c.id == id).FirstOrDefault();
            ViewBag.TaskInfo = CurrentTask;
            var AnswerList = repAnswers.GetDapperAnswerList().Where(c => c.TaskId == id).ToList();
            ViewBag.AnswerList = AnswerList;
            ViewBag.TaskDocs = db.TaskDocs.Where(c => c.TaskId == id).ToList();
            var vmanswer = new VMExpertAnswer() { 
             TaskId=id,
             
            };
            return View(vmanswer);
        }
        public ActionResult SendAnswer(int id)
        {

            var CurrentTask=repTask.GetDapperTaskList().Where(c=>c.id==id).FirstOrDefault();
            ViewBag.TaskInfo = CurrentTask;
            VMExpertAnswer vmanswer = new VMExpertAnswer() { 
            
              TaskId=CurrentTask.id
            };
            return View(vmanswer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendAnswer(VMExpertAnswer vmAnswer)
        {

            var CurrentTask = repTask.GetDapperTaskList().Where(c => c.id ==vmAnswer.TaskId).FirstOrDefault();
            var CurrentAdmin=db.AspNetUsers.Where(c=>c.Id==CurrentTask.AdminId).FirstOrDefault();
            ViewBag.TaskInfo = CurrentTask;
          if(!ModelState.IsValid)
          {
              return View(vmAnswer);
          }

          var newanswer = new TaskAnswer() {
          
           TaskId=CurrentTask.id,
            AnswerDate=DateTime.Now,
             ExpertId=User.Identity.GetUserId(),
              Descriptions=vmAnswer.Descriptions,
               Title=vmAnswer.Title
          };
          repAnswers.Insert(newanswer);
          int lastid = repAnswers.GetAll().Max(c => c.id);


          InsertFiles(lastid, vmAnswer.SelectedFiles);
            var CurrentExpert=repUserInfo.GetByUserID(User.Identity.GetUserId())
; string strMsg = "یک پاسخ برای وظیفه توسط : " + CurrentExpert.Name + " ثبت شد.عنوان وظیفه : " + CurrentTask.Title;
            Helpers.SMSHelper.SendSMS(CurrentAdmin.UserName, "", strMsg);
            return RedirectToAction("Details", new { id=CurrentTask.id});
        }


      
        private void InsertFiles(int AnswerId, string Files)
        {
            if (string.IsNullOrEmpty(Files))
            {
                return;
            }
            List<ViewModels.VMSelectedFiles> vmSekectedFiles = JsonConvert.DeserializeObject<List<ViewModels.VMSelectedFiles>>(Files);
            var db = new DB_AlmaCmsEntities();


            foreach (var item in vmSekectedFiles)
            {

                var newfile = new Models.TaskAnwerDoc()
                {
                    FileName = item.FileName,
                    Title = item.Title,
                    TaskAnswerId = AnswerId
                };

                db.TaskAnwerDocs.Add(newfile);
                db.SaveChanges();
            }



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UploadFile()
        {

            try
            {
                var file = Request.Files[0];


                if (!Helpers.HttpPostedFileBaseExtensions.IsImage(file))
                {
                    return Json(new { State = "false", Filename = "", Message = "فایل انتخابی معتبر نمی باشد." });
                }

                if (file.ContentLength < 3000000)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string guID = Guid.NewGuid().ToString();
                    string nameIS = guID + extension;
                    file.SaveAs(Server.MapPath("/UploadedFiles/" + nameIS));

                    return Json(new { State = "true", Filename = nameIS, Message = "فایل با موفقیت بار گزاری شد" });


                }
                else
                {


                    return Json(new { State = "false", Filename = "", Message = "سایز فایل از 3 مگابایت بیشتر است" });

                }

            }

            catch (Exception)
            {
                return Json(new { State = "false", Filename = "", Messgae = "خطا در بارگزاری فایل" });
            }




        }

    }
}