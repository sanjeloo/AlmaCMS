using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using System.Net;
namespace AlmaCMS.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageStateController : Controller
    {
        public static bool Premission = false;
        private DB_AlmaCmsEntities DB;
        private StateRepository RepState;


        public ManageStateController()
        {
            DB = new DB_AlmaCmsEntities();
            RepState = new StateRepository(DB);
           
        }
        // GET: AdminShop/ManageState
        public ActionResult Index()
        {
            var StateList = RepState.GetAll();
            var vmState = new List<VMState>();
            foreach (var item in StateList)
            {
                vmState.Add(item.toVMState());
            }
            return View(vmState);
        }


        public ActionResult Create()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMState vmstate)
        {
            if (!ModelState.IsValid)
            {
                return View(vmstate);
            }
            RepState.Insert(vmstate.toState());
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int? id)
        {
            //ViewBag.ParrentPage = RepState.Where(c => c.id != id).ToList();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CurrentState = RepState.FindById((int)id);
            if (CurrentState == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(CurrentState.toVMState());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMState vmstate)
        {
            if (!ModelState.IsValid)
            {
                return View(vmstate);
            }
            RepState.Update(vmstate.toState());
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CurrentStaste = RepState.FindById((int)id);
            if (CurrentStaste == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        
            return View(CurrentStaste.toVMState());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentStaste = RepState.FindById(id);
            RepState.Delete(id);
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