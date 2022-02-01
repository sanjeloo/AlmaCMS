using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using PagedList;
using PagedList.Mvc;

using System.Web.Routing;
using System.Web.Security;

namespace AlmaCMS.Areas.Manage.Controllers
{

    public class ManageCertificateGroupController : Controller
    {
             public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private CertificateGroupRepository repCertificateGroup;
        CertificateRepository repCertificate;
        public ManageCertificateGroupController()
        {
            db = new DB_AlmaCmsEntities();
            repCertificateGroup = new CertificateGroupRepository(db);
            repCertificate = new CertificateRepository(db);
        }

        // GET: Manage/ManageMenuItems
        public ActionResult Index(string searchString, int? page)
        {
            var vmGroupList = new List<VMCertificateGroup>();
            var groupList = repCertificateGroup.GetAll().ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                groupList = groupList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }
            foreach (var item in groupList)
            {
                vmGroupList.Add(item.toVMCertificateGroup());
            }
      
            return View(vmGroupList.ToList());

        }

        #region Create
        public ActionResult Create()
        {




            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMCertificateGroup vmGroup)
        {
         
            if (ModelState.IsValid)
            {



                repCertificateGroup.Insert(vmGroup.toCertificateGroup());
                Helpers.UserLogHelper.AddLog("گروه کواهینامه ها", (User.Identity.Name), "درج", vmGroup.Title);
                return RedirectToAction("Index");
            }

            return View(vmGroup);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repCertificateGroup.Where(c => c.id != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repCertificateGroup.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentpage.toVMCertificateGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMCertificateGroup vmCertificategroup)
        {
          
            if (!ModelState.IsValid)
                return View(vmCertificategroup);




            repCertificateGroup.Update(vmCertificategroup.toCertificateGroup());
            Helpers.UserLogHelper.AddLog("گروه کواهینامه ها", (User.Identity.Name), "ویرایش", vmCertificategroup.Title);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repCertificateGroup.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            if (repCertificate.getByGroupID(currentPage.id).ToList().Count > 0)
            {

                ViewBag.ErrorMessage = "امکان حذف گروه گواهینامه ها وجود ندارد. گروه دارای گواهینامه می باشد";


            }
            return View(currentPage.toVMCertificateGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repCertificateGroup.FindById(id);
            repCertificateGroup.Delete(id);
            Helpers.UserLogHelper.AddLog("گروه کواهینامه ها", (User.Identity.Name), " حذف ", CurrentItem.Title);
            return RedirectToAction("Index");
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
  
}