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
   
    public class ManageMenuItemsController : Controller
    {
        public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private MenuItemRepository repMenuItem;

        // GET: Manage/Managepages

        public ManageMenuItemsController()
        {
            db = new DB_AlmaCmsEntities();
            repMenuItem = new MenuItemRepository(db);

        }

        // GET: Manage/ManageMenuItems
        public ActionResult Index(string searchString, int? page)
        {
            var VMpagesList = new List<VMMenuItems>();
            var MenuList = repMenuItem.Where(c => c.id != 0).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                MenuList = MenuList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }
            foreach (var item in MenuList)
            {
                VMpagesList.Add(item.toVMMenuItems());
            }
            return View(VMpagesList);

        }

        #region Create
        public ActionResult Create()
        {
            var vmMenu = new VMMenuItems();
            ViewBag.ParentPages = repMenuItem.GetAll();



            return View(vmMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMMenuItems vmMenu)
        {
            ViewBag.ParentPages = repMenuItem.GetAll();
            if (ModelState.IsValid)
            {



                repMenuItem.Insert(vmMenu.toMenuItem());
                Helpers.UserLogHelper.AddLog("منوی اصلی", (User.Identity.Name), "درج", vmMenu.Title);

                return RedirectToAction("Index");
            }

            return View(vmMenu);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repMenuItem.Where(c => c.id != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repMenuItem.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentpage.toVMMenuItems());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMMenuItems vmMenu)
        {
            ViewBag.ParentPages = repMenuItem.Where(c => c.id != vmMenu.id).ToList();
            if (!ModelState.IsValid)
                return View(vmMenu);

            var currentPAge = repMenuItem.FindById(vmMenu.id);
            currentPAge.Comments = vmMenu.Comments;
            currentPAge.DirectUrl = vmMenu.DirectUrl;
            currentPAge.IsPrimary = vmMenu.IsPrimary;
            //currentPAge.Name = vmMenu.Name;
            currentPAge.OpenInNew = vmMenu.OpenInNew;
            currentPAge.ParentID = vmMenu.ParentID;
            currentPAge.Title = vmMenu.Title;
            currentPAge.Priority = vmMenu.Priority;


            repMenuItem.Update(currentPAge);
            Helpers.UserLogHelper.AddLog("منوی اصلی", (User.Identity.Name), "ویرایش", vmMenu.Title);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repMenuItem.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            if (repMenuItem.GetByParentID(currentPage.id).ToList().Count > 0)
            {

                ViewBag.ErrorMessage = "امکان حذف منو وجود ندارد.منو دارای زیر منو می باشد";


            }
            return View(currentPage.toVMMenuItems());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repMenuItem.FindById(id);
            repMenuItem.Delete(id);
            Helpers.UserLogHelper.AddLog("منوی اصلی", (User.Identity.Name), "حذف", CurrentItem.Title);

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