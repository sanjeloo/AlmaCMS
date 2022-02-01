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
        [HasPermission("ManageProductReports")]
    public class ManageProductReportsController : Controller
    {
        DB_AlmaCmsEntities db;
        UserInfoRepositiry repUserInfo;

        //ProductReportre repTasks;
        ProductReportRepository repReport;
        CustomeProductsRepository repProduct;
        ProductReportAnswerRepository repAnswer;
            public  ManageProductReportsController()
        {
            db = new DB_AlmaCmsEntities();
            repUserInfo = new UserInfoRepositiry(db);
            repReport = new ProductReportRepository(db);
            repProduct = new CustomeProductsRepository(db);
            repAnswer = new ProductReportAnswerRepository(db);
                
        }

            public ManageProductReportsController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        // GET: Manage/ManageProductReports
        public ActionResult Index()
        {
            return View(repReport.GetDapperReportList().OrderByDescending(c=>c.id).ToList());
        }

        public ActionResult Create()
        {

            var Currentrole = RoleManager.FindByName("Expert");
            string RoleID = Currentrole.Id;

            var newTask = new VMProductReport()
            {
                AdminId = User.Identity.GetUserId(),
                DateInsert = DateTime.Now,

            };

            var VMProductList = new List<VMDropDownOption>();
            var vmUserList = new List<VMDropDownOption>();
            var ProductList = repProduct.GetAll().OrderBy(c=>c.Title).ToList();
            var Userist = repUserInfo.getDapperUserInfo().Where(c => c.RoleId == RoleID).ToList();
            VMProductList.Add(new VMDropDownOption()
            {
                Id = 0,
                Title = "انتخاب محصول"
            });
            foreach (var item in ProductList)
            {
                VMProductList.Add(new VMDropDownOption() { 
                 Id=item.id,
                  Title=item.Title
                });
            }
            ViewBag.ProductList = VMProductList;
            var featuresItems = Userist.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            ViewBag.featuresItems = featuresItems;


            return View(newTask);

        }


        [HttpPost]
        public ActionResult Create(VMProductReport vmReport)
        {

            var Currentrole = RoleManager.FindByName("Expert");
            string RoleID = Currentrole.Id;

            var newTask = new VMProductReport()
            {
                AdminId = User.Identity.GetUserId(),
                DateInsert = DateTime.Now,

            };

            var VMProductList = new List<VMDropDownOption>();
            var vmUserList = new List<VMDropDownOption>();
            var ProductList = repProduct.GetAll().OrderBy(c => c.Title).ToList();
            var Userist = repUserInfo.getDapperUserInfo().Where(c => c.RoleId == RoleID).ToList();

            foreach (var item in ProductList)
            {
                VMProductList.Add(new VMDropDownOption()
                {
                    Id = item.id,
                    Title = item.Title
                });
            }
            ViewBag.ProductList = VMProductList;
            var featuresItems = Userist.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            ViewBag.featuresItems = featuresItems;


            if (!ModelState.IsValid)
            {
                return View(vmReport);
            }

            var newReport = new Models.ProductReport()
            {
                AdminId = vmReport.AdminId,
                DateInsert = DateTime.Now,
                Descriptions = vmReport.Descriptions,
                Title = vmReport.Title
                ,
               
                StatusId = 1,
               ProductId=vmReport.ProductId,
                

            };
            repReport.Insert(newReport);
            var lastid = repReport.getLastid();
            InsertFeatures(lastid, vmReport.SelectedFeatures);

           
            InsertFiles(lastid, vmReport.SelectedFiles);

            SendSms(lastid, vmReport.SelectedFeatures);
            return RedirectToAction("index");


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
                        ProductReportExpert NewRole = new ProductReportExpert();
                        //NewRole.Id = int.Parse(item);
                        NewRole.ProductReportId = TaskId;
                        //var roleId = db.Roles.FirstOrDefault(r => r.Id == int.Parse(item));
                        NewRole.UserId = (item);
                        db.ProductReportExperts.Add(NewRole);
                        db.SaveChanges();
                    }
                }
            }

        }

        private void SendSms(int TaskId, string Users)
        {
            var db = new DB_AlmaCmsEntities();

            var CurrntTask = repReport.FindById(TaskId);
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
                            string strMessage = "کاربر گرامی " + currentuser.Name + " یک پیگیری گزارش محصول در تاریخ  " + DateTime.Now.ToPersianShortDate() + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + " با عنوان " + CurrntTask.Title +
                              " برای شما ثبت شد";
                            SMSHelper.SendSMS(currentuser.Mobile, "گزارش محصول", strMessage);
                        }
                    }
                }
            }

        }

        private void InsertFiles(int ReportId, string Files)
        {
            if(string.IsNullOrEmpty(Files))
            {
                return;
            }
            List<ViewModels.VMSelectedFiles> vmSekectedFiles = JsonConvert.DeserializeObject<List<ViewModels.VMSelectedFiles>>(Files);
            var db = new DB_AlmaCmsEntities();


            foreach (var item in vmSekectedFiles)
            {

                var newfile = new ProductReportDoc()
                {
                    FileName = item.FileName,
                    Title = item.Title,
                    ProductReportId = ReportId
                };

                db.ProductReportDocs.Add(newfile);
                db.SaveChanges();
            }



        }

        #region UploadFile


        /*آپلود فایل */
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
        #endregion



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
            VWProducReport vmReport = repReport.GetDapperReportList().Where(c => c.id == id).FirstOrDefault();
            if (vmReport == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReportInfo = vmReport;
            ViewBag.ReportDocs = db.ProductReportDocs.Where(c => c.ProductReportId == id).ToList();
            var AnswerList = repAnswer.GetDapperAnswerList().Where(c => c.ProductReportId == id).ToList();
            ViewBag.AnswerList = AnswerList;
            ViewBag.StatusList = db.ProductReportStatus.ToList();
            return View(vmReport);
        }

        public ActionResult ChangeState(int statusID, int ReportId)
        {

            var currentOrder = repReport.FindById(ReportId);
            currentOrder.StatusId = statusID;
            repReport.Update(currentOrder);
            return RedirectToAction("index");
        }


        public ActionResult Delete(int id)
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMembers.GetByEmail(User.Identity.Name).MemberID, "DeleteOrder"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VWProducReport vmtask = repReport.GetDapperReportList().Where(c => c.id == id).FirstOrDefault();
            if (vmtask == null)
            {
                return HttpNotFound();
            }

            ViewBag.TaskInfo = vmtask;
            ViewBag.TaskDocs = db.ProductReportDocs.Where(c => c.ProductReportId == id).ToList();

            ViewBag.StatusList = db.ProductReportStatus.ToList();


            return View(vmtask);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {


            repReport.Delete(id);

            return RedirectToAction("Index");
        }

        public ActionResult SendAnswer(int id)
        {

            var CurrentReport = repReport.GetDapperReportList().Where(c => c.id == id).FirstOrDefault();
            ViewBag.ReporttInfo = CurrentReport;
            VMReportExpertAnswer vmanswer = new VMReportExpertAnswer()
            {

                ProductReportId = CurrentReport.id
            };
            return View(vmanswer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendAnswer(VMReportExpertAnswer vmAnswer)
        {

            var CurrentReport = repReport.GetDapperReportList().Where(c => c.id == vmAnswer.ProductReportId).FirstOrDefault();
            ViewBag.ReportInfo = CurrentReport;
            if (!ModelState.IsValid)
            {
                return View(vmAnswer);
            }

            var newanswer = new ProductReportAnswer()
            {

                ProductReportId = CurrentReport.id,
                AnswerDate = DateTime.Now,
                UserId = User.Identity.GetUserId(),
                Descriptions = vmAnswer.Descriptions,
                Title = vmAnswer.Title
            };
            repAnswer.Insert(newanswer);
            int lastid = repAnswer.GetAll().Max(c => c.id);
            InsertAnswerFiles(lastid, vmAnswer.SelectedFiles);
            return RedirectToAction("Details", new { id = CurrentReport.id });

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

                var newfile = new Models.ProductReportAnswerDoc()
                {
                    FileName = item.FileName,
                    Title = item.Title,
                    ProductReportAnswerId = AnswerId
                };

                db.ProductReportAnswerDocs.Add(newfile);
                db.SaveChanges();
            }



        }
    }
}