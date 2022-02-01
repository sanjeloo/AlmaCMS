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
    [Authorize(Roles = "Admin")]
    [HasPermission("ManagePages")]
    public class ManagePagesController : Controller
    {
        private DB_AlmaCmsEntities db;
        private PagesRepository repPages;
        public static bool premission = false;
        // GET: Manage/Managepages

        public ManagePagesController()
        {
            db = new DB_AlmaCmsEntities();
            repPages = new PagesRepository(db);

        }
        public ActionResult Index()
        {
            var VMpagesList = new List<VMPages>();
            var pagesList = repPages.Where(c => c.PegeID != 0).ToList();

            foreach (var item in pagesList)
            {
                VMpagesList.Add(item.toVMPages());
            }


            return View(VMpagesList.ToList());

        }

        #region Create
        public ActionResult Create()
        {
            var vmpage = new VMPages();
            ViewBag.ParentPages = repPages.GetAll();


            return View(vmpage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMPages vmPage)
        {
            ViewBag.ParentPages = repPages.GetAll();
            if (ModelState.IsValid)
            {
                if (vmPage.KeyWords == null)
                    vmPage.KeyWords = "";
                vmPage.UniqID = Guid.NewGuid().ToString();
                vmPage.dateInsert = DateTime.Now;

                repPages.Insert(vmPage.toPage());
                Helpers.UserLogHelper.AddLog("مدیریت صفحات", (User.Identity.Name), "درج", vmPage.Title);

                return RedirectToAction("Index");
            }

            return View(vmPage);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repPages.Where(c => c.PegeID != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repPages.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            ViewBag.PageGuid = currentpage.UniqID;
            return View(currentpage.toVMPages());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMPages vmPage)
        {
            ViewBag.ParentPages = repPages.Where(c => c.PegeID != vmPage.PageID).ToList();
            if (!ModelState.IsValid)
                return View(vmPage);

            var currentPAge = repPages.FindById(vmPage.PageID);
            currentPAge.DirectLink = vmPage.DirectUrl;
            currentPAge.ImageDesc = vmPage.ImageDesc;
            currentPAge.ImageTitle = vmPage.ImageTitle;
            currentPAge.ImageUrl = vmPage.ImageUrl;
            currentPAge.Keywords = vmPage.KeyWords;
            currentPAge.PageContent = vmPage.PageContent;
            currentPAge.PageName = vmPage.PageName;

            currentPAge.ShortDesc = vmPage.ShortDesc;
            currentPAge.ShowInMenu = vmPage.ShowInMenu;
            currentPAge.Title = vmPage.Title;

            repPages.Update(currentPAge);
            Helpers.UserLogHelper.AddLog("مدیریت صفحات", (User.Identity.Name), "ویرایش", vmPage.Title);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repPages.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentPage.toVMPages());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repPages.FindById(id);
            repPages.Delete(id);
            Helpers.UserLogHelper.AddLog( "مدیریت صفحات",(User.Identity.Name), "حذف", CurrentItem.Title);

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