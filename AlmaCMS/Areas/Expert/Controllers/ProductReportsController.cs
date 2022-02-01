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
    public class ProductReportsController : Controller
    {
              DB_AlmaCmsEntities db;
        ProductReportRepository repReport;
        UserInfoRepositiry repUserInfo;
        ProductReportAnswerRepository repAnswers;
        public ProductReportsController()
        {
            db = new DB_AlmaCmsEntities();
            repReport = new  ProductReportRepository(db);
            repUserInfo = new UserInfoRepositiry(db);
            repAnswers = new  ProductReportAnswerRepository(db);
        }
        // GET: Expert/Taks
        public ActionResult Index()
        {

            var Userif = User.Identity.GetUserId();
            var ProductReport = repReport.GetDapperExpertReportList().Where(c => c.UserId == Userif).Where(c=>c.StatusId!=4).ToList();

            return View(ProductReport);
        }

        public ActionResult Details(int id)
        {
            var CurrentReport = repReport.GetDapperReportList().Where(c => c.id == id).FirstOrDefault();
            ViewBag.ReportInfo = CurrentReport;
            var AnswerList = repAnswers.GetDapperAnswerList().Where(c => c.ProductReportId == id).ToList();
            ViewBag.AnswerList = AnswerList;
            ViewBag.ReportDocs = db.ProductReportDocs.Where(c => c.ProductReportId == id).ToList();
            var vmanswer = new VMReportExpertAnswer() { 
             ProductReportId=id,
             
            };
            return View(vmanswer);
        }
        public ActionResult SendAnswer(int id)
        {

            var CurrentReport=repReport.GetDapperReportList().Where(c=>c.id==id).FirstOrDefault();
            ViewBag.ReporttInfo = CurrentReport;
            VMReportExpertAnswer vmanswer = new VMReportExpertAnswer()
            { 
            
              ProductReportId=CurrentReport.id
            };
            return View(vmanswer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendAnswer(VMReportExpertAnswer vmAnswer)
        {

            var CurrentReport = repReport.GetDapperReportList().Where(c => c.id ==vmAnswer.ProductReportId).FirstOrDefault();
            ViewBag.ReportInfo = CurrentReport;
          if(!ModelState.IsValid)
          {
              return View(vmAnswer);
          }

          var newanswer = new ProductReportAnswer() {
          
           ProductReportId=CurrentReport.id,
            AnswerDate=DateTime.Now,
             UserId=User.Identity.GetUserId(),
              Descriptions=vmAnswer.Descriptions,
               Title=vmAnswer.Title
          };
          repAnswers.Insert(newanswer);
          int lastid = repAnswers.GetAll().Max(c => c.id);
          InsertFiles(lastid, vmAnswer.SelectedFiles);
          return RedirectToAction("Details", new { id=CurrentReport.id});
 
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