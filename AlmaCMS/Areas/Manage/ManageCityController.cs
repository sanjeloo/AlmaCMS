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

namespace AlmaCMS.Areas.Manage
{
    [Authorize(Roles = "Admin")]
    public class ManageCityController : Controller
    {
        public static bool Premission = false;
        private DB_AlmaCmsEntities DB;
        private StateRepository RepState;
        private CityRepository RepCity;



        public ManageCityController()
        {
            DB = new DB_AlmaCmsEntities();
            RepState = new StateRepository(DB);
            RepCity = new CityRepository(DB);

        }

        // GET: AdminShop/ManageCity
        public ActionResult Index(int id)
        {
            var vmcity = new List<VMCity>();

            var CityList = RepCity.getByGroupID(id);


            foreach (var item in CityList)
            {
                vmcity.Add(item.toVMCity());
            }
            var CurrentState = RepState.FindById(id);

            if (CurrentState != null)
                ViewBag.GroupTitle = CurrentState.Title;


            ViewBag.StateID = CurrentState.id;

            return View(vmcity.ToList());
        }



        public ActionResult Create(int id)
        {

            var vmcity = new VMCity();
            ViewBag.GroupID = id;
            vmcity.StateID = id;
            return View(vmcity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMCity vmcity)
        {
            if (!ModelState.IsValid)
            {
                return View(vmcity);
            }
            RepCity.Insert(vmcity.toCity());
            return RedirectToAction("Index", new { id = vmcity.StateID });

        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CurrentCity = RepCity.FindById((int)id);
            if (CurrentCity == null)
            {
                return HttpNotFound();
            }
            return View(CurrentCity.toVMCity());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMCity vmcity)
        {
            if (!ModelState.IsValid)
            {
                return View(vmcity);
            }
            RepCity.Update(vmcity.toCity());
            return RedirectToAction("Index", new { id = vmcity.StateID });

        }



        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CurrentCity = RepCity.FindById((int)id);
            if (CurrentCity == null)
            {
                return HttpNotFound();
            }
           
            return View(CurrentCity.toVMCity());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentCity = RepCity.FindById(id);

            int GroupID = (int)RepCity.FindById(id).StateID;
            RepCity.Delete(id);
            return RedirectToAction("Index", new { id = GroupID });
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