using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AlmaCMS.Models;
using AlmaCMS.Repositories;
using AlmaCMS.ViewModels;
using AlmaCMS.Helpers;
using System.Net;

namespace AlmaCMS.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]

    [HasPermission("ManageSliders")]
    public class ManageSliderController : Controller
    {

        public static bool Premission = false;
        private DB_AlmaCmsEntities DB;
        private SliderRepository RepSlider;


        public ManageSliderController()
        {
            DB = new DB_AlmaCmsEntities();
            RepSlider = new SliderRepository(DB);

        }
        // GET: AdminShop/ManageSlider
        public ActionResult Index()
        {
            var vmslider = new List<VMSlider>();
            var SliderList = RepSlider.GetAll();
            foreach (var item in SliderList)
            {
                vmslider.Add(item.toVMSlider());
            }
            return View(vmslider);
        }


        public ActionResult Create()
        {

            VMSlider newslider = new VMSlider() { Priority = 0 };
            return View(newslider);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMSlider vmslider)
        {
            if (!ModelState.IsValid)
            {
                return View(vmslider);
            }
            RepSlider.Insert(vmslider.toSlider());
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CurrentSlider = RepSlider.FindById((int)id);
            if (CurrentSlider == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(CurrentSlider.toVMSlider());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMSlider vmslider)
        {
            if (!ModelState.IsValid)
            {
                return View(vmslider);
            }
            RepSlider.Update(vmslider.toSlider());
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CurrentSlider = RepSlider.FindById((int)id);
            if (CurrentSlider == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(CurrentSlider.toVMSlider());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            RepSlider.Delete(id);
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DB.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}