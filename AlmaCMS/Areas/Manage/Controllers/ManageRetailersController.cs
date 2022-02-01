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

    public class ManageRetailersController : Controller
    {
       
        // GET: Manage/ManageRetailers
        private DB_AlmaCmsEntities db;
        private RetailerRepository repRetailer;
     
        public static bool premission = false;
        public ManageRetailersController()
        {
            db = new DB_AlmaCmsEntities();
            repRetailer = new RetailerRepository(db);
           
           
        }


        #region Index
        public ActionResult Index()
        {
            var vmRetailerList = new List<VMRetailers>();

            var RetailerList = repRetailer.GetAll();



            foreach (var item in RetailerList)
            {
                vmRetailerList.Add(item.toVMRetailers());
            }
     
            
            
            return View(vmRetailerList);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            VMRetailers vmRetailer = new VMRetailers();
            
            return View(vmRetailer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( VMRetailers vmRetailer)
        {
            if (ModelState.IsValid)
            {
               
                repRetailer.Insert(vmRetailer.toRetailer());
                Helpers.UserLogHelper.AddLog("نمایندگان", (User.Identity.Name), "درج", vmRetailer.Name);

                return RedirectToAction("Index");
            }

            return View(vmRetailer);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Retailer Retailer = repRetailer.FindById((int)id);
            if (Retailer == null)
            {
                return HttpNotFound();
            }
            return View(Retailer.toVMRetailers());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( VMRetailers vmRetailer)
        {
            if (ModelState.IsValid)
            {
                repRetailer.Update(vmRetailer.toRetailer());
                Helpers.UserLogHelper.AddLog("نمایندگان", (User.Identity.Name), "ویرایش", vmRetailer.Name);

                return RedirectToAction("Index");
            }
            return View(vmRetailer);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Retailer Retailer = repRetailer.FindById((int)id);
            if (Retailer == null)
            {
                return HttpNotFound();
            }
            return View(Retailer.toVMRetailers());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var CurrentItem = repRetailer.FindById(id);
            repRetailer.Delete(id);
            Helpers.UserLogHelper.AddLog("نمایندگان", (User.Identity.Name), "حذف", CurrentItem.Name);

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