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
    [Authorize(Roles = "Expert")]
    public class MyTasksController : Controller
    {
        // GET: Expert/MyTasks
   

        public ActionResult Create()
        {

            var Currentrole = RoleManager.FindByName("Expert");
            string RoleID = Currentrole.Id;

            var newTask = new VMTask()
            {
                AdminId = User.Identity.GetUserId(),
                DateInsert = DateTime.Now,

            };
            var vmUserList = new List<VMDropDownOption>();
            var CurrentUserId = User.Identity.GetUserId();
            var ExpertSubUSers = db.ExpertSubUsers.Where(c => c.ExpertId == CurrentUserId).ToList();
            var userInfoList = repUserInfo.getDapperUserInfo().Where(c => ExpertSubUSers.Select(s => s.SubUserId).Contains(c.Id)).ToList();
            var featuresItems = userInfoList.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            ViewBag.featuresItems = featuresItems;


            return View(newTask);

        }

         DB_AlmaCmsEntities db;
        TasksRepository repTask;
        UserInfoRepositiry repUserInfo;
        TaskAnswerRepository repAnswers;
        public MyTasksController()
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
            return View(repTask.GetDapperTaskList().Where(c=>c.AdminId==Userif).OrderByDescending(c => c.id).ToList());

      
        }

        public ActionResult Details(int id)
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMembers.GetByEmail(User.Identity.Name).MemberID, "DetailOrder"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VWTaskList vmtask = repTask.GetDapperTaskList().Where(c => c.id == id).FirstOrDefault();
            if (vmtask == null)
            {
                return HttpNotFound();
            }

            ViewBag.TaskInfo = vmtask;
            ViewBag.TaskDocs = db.TaskDocs.Where(c => c.TaskId == id).ToList();
            var AnswerList = repAnswers.GetDapperAnswerList().Where(c => c.TaskId == id).ToList();
            ViewBag.AnswerList = AnswerList;
            ViewBag.StatusList = db.TaskStatus.ToList();
            return View(vmtask);
        }
        public ActionResult ChangeState(int statusID, int TaskId)
        {

            var currentOrder = repTask.FindById(TaskId);
            currentOrder.StatusId = statusID;
            repTask.Update(currentOrder);
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult Create(VMTask vmtask)
        {

            var Currentrole = RoleManager.FindByName("Expert");
            string RoleID = Currentrole.Id;


            var vmUserList = new List<VMDropDownOption>();
            var CurrentUserId = User.Identity.GetUserId();
            var ExpertSubUSers = db.ExpertSubUsers.Where(c => c.ExpertId == CurrentUserId).ToList();
            var userInfoList = repUserInfo.getDapperUserInfo().Where(c => ExpertSubUSers.Select(d=>d.SubUserId).Contains(c.Id)).ToList();
            var featuresItems = userInfoList.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            ViewBag.featuresItems = featuresItems;

            if (!ModelState.IsValid)
            {
                return View(vmtask);
            }

            var newtask = new Models.TasksList()
            {
                AdminId = vmtask.AdminId,
                DateInsert = DateTime.Now,
                Descriptions = vmtask.Descriptions,
                ExpireDate = DateTime.Now.AddDays(vmtask.AdDays).AddHours(vmtask.adHour).AddMinutes(vmtask.adMinute),
                Title = vmtask.Title
                ,
                TimeRequired = (vmtask.AdDays * 1440) + (vmtask.adHour * 60) + vmtask.adMinute,
                StatusId = 1,
                SendSMSAlarm = vmtask.SendAlarmSMS

            };
            repTask.Insert(newtask);
            var lastid = repTask.getLastid();
            InsertFeatures(lastid, vmtask.SelectedFeatures);
            InsertFiles(lastid, vmtask.SelectedFiles);

            SendSms(lastid, vmtask.SelectedFeatures);
            return RedirectToAction("index");


        }
            private void InsertFiles(int TaskId, string Files)
        {
            if (string.IsNullOrEmpty(Files))
            {
                return;
            }

            List<ViewModels.VMSelectedFiles> vmSekectedFiles = JsonConvert.DeserializeObject<List<ViewModels.VMSelectedFiles>>(Files);
            var db = new DB_AlmaCmsEntities();


            foreach (var item in vmSekectedFiles)
            {
                
                var newfile = new TaskDoc() { 
                 FileName=item.FileName,
                  Title=item.Title,
                   TaskId=TaskId
                };

                db.TaskDocs.Add(newfile);
                db.SaveChanges();
            }
          
           

        }

                public ActionResult SendAnswer(int id)
        {

            var CurrentTask = repTask.GetDapperTaskList().Where(c => c.id == id).FirstOrDefault();
            ViewBag.TaskInfo = CurrentTask;
            VMExpertAnswer vmanswer = new VMExpertAnswer()
            {

                TaskId = CurrentTask.id
            };
            return View(vmanswer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendAnswer(VMExpertAnswer vmAnswer)
        {

            var CurrentTask = repTask.GetDapperTaskList().Where(c => c.id == vmAnswer.TaskId).FirstOrDefault();
            ViewBag.TaskInfo = CurrentTask;
            if (!ModelState.IsValid)
            {
                return View(vmAnswer);
            }

            var newanswer = new TaskAnswer()
            {

                TaskId = CurrentTask.id,
                AnswerDate = DateTime.Now,
                ExpertId = User.Identity.GetUserId(),
                Descriptions = vmAnswer.Descriptions,
                Title = vmAnswer.Title
            };
            repAnswers.Insert(newanswer);
            int lastid = repAnswers.GetAll().Max(c => c.id);
            InsertAnswerFiles(lastid, vmAnswer.SelectedFiles);
            return RedirectToAction("Details", new { id = CurrentTask.id });
        }
  
        private void InsertAnswerFiles(int AnswerId, string Files)
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
        private void SendSms(int TaskId, string Users)
        {
            var db = new DB_AlmaCmsEntities();
            var CurrntTask = repTask.FindById(TaskId);

            if (Users != "" && Users != null)
            {
                var ArrRoles = Users.Split(',');
                if (ArrRoles[ArrRoles.Length - 1] == "" || ArrRoles[ArrRoles.Length - 1] == null)
                {
                    ArrRoles = ArrRoles.Where(a => a != "" && a != null).ToArray();
                }
                foreach (var item in ArrRoles)
                {
                    if (item != "")
                    {
                        var currentuser = repUserInfo.GetByUserID(item);
                        if (currentuser != null)
                        {
                            string strMessage = "کاربر گرامی " + currentuser.Name + " یک وظیفه در تاریخ  " + DateTime.Now.ToPersianShortDate() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " با عنوان " + CurrntTask.Title;

                            if (CurrntTask.SendSMSAlarm == true)
                            {
                                strMessage += " با مهلت انجام " + CurrntTask.ExpireDate.Value.ToPersianShortDate() + " " + CurrntTask.ExpireDate.Value.Hour + ":" + CurrntTask.ExpireDate.Value.Minute;

                            }
                            strMessage += " برای شما ثبت شد";
                            SMSHelper.SendSMS(currentuser.Mobile, "وظیفه", strMessage);
                        }
                    }
                }
            }

        }

        private void InsertFeatures(int TaskId, string Users)
        {
            var db = new DB_AlmaCmsEntities();
            db.TaskExperts.RemoveRange(db.TaskExperts.Where(fs => fs.TaskId == TaskId));
            db.SaveChanges();
            if (Users != "" && Users != null)
            {
                var ArrRoles = Users.Split(',');
                if (ArrRoles[ArrRoles.Length - 1] == "" || ArrRoles[ArrRoles.Length - 1] == null)
                {
                    ArrRoles = ArrRoles.Where(a => a != "" && a != null).ToArray();
                }
                foreach (var item in ArrRoles)
                {
                    if (item != "")
                    {
                        TaskExpert NewRole = new TaskExpert();
                        //NewRole.Id = int.Parse(item);
                        NewRole.TaskId = TaskId;
                        //var roleId = db.Roles.FirstOrDefault(r => r.Id == int.Parse(item));
                        NewRole.UseId = (item);
                        db.TaskExperts.Add(NewRole);
                        db.SaveChanges();
                    }
                }
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