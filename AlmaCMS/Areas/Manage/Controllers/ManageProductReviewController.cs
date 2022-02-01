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
    public class ManageProductReviewController : Controller
    {
        // GET: AdminShop/ManageProductReview

        private DB_AlmaCmsEntities db;
        private ProductReviewRepository RepREviews;
        public ManageProductReviewController()
        {

            db = new DB_AlmaCmsEntities();
            RepREviews = new ProductReviewRepository(db);
            //repMember = new MembersRepository(db);
        }
        // GET: Manage/ManageArticleReview
        public ActionResult Index()
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMember.GetByEmail(User.Identity.Name).MemberID, "ManageArticleReview"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}

            var Reviewslist = RepREviews.GetReviews();

            return View(Reviewslist.OrderByDescending(c => c.Id).ToList());
        }


        #region edit
        public ActionResult Edit(int? Id)
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMember.GetByEmail(User.Identity.Name).MemberID, "EditArticleReviews"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}
            if (Id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentreview = RepREviews.FindById((int)Id);
            if (currentreview == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentreview.toVMpRODUCTReviews());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMArticleReviews vmarticleview)
        {
            if (!ModelState.IsValid)
                return View(vmarticleview);
            var currentreview = RepREviews.FindById(vmarticleview.Id);
            currentreview.Name = vmarticleview.Name;
            currentreview.Title = vmarticleview.Title;
            currentreview.Comment = vmarticleview.Comment;
            currentreview.Email = vmarticleview.Email;
            currentreview.Answer = vmarticleview.Answer;


            RepREviews.Update(currentreview);
            //Helpers.UserLogHelper.AddLog("مدیریت صفحات", (User.Identity.Name), "ویرایش", vmPage.Title);

            return RedirectToAction("Index");

        }
        #endregion
        public ActionResult Delete(int? id)
        {


            //if (!NSShop.Helpers.UserRole.IsUserRole(repMember.GetByEmail(User.Identity.Name).MemberID, "DeleteArticleReviews"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var CurrentReviews = RepREviews.FindById((int)id);
            if (CurrentReviews == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(CurrentReviews);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id)
        {
            var currentitem = RepREviews.FindById((int)id);
            RepREviews.Delete((int)id);

            return RedirectToAction("index");
        }


        public ActionResult DoActive(int? id)
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMember.GetByEmail(User.Identity.Name).MemberID, "DoactiveReviews"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}
            var currentdoactive = RepREviews.FindById((int)id);
            if (currentdoactive.Status == false)
            {
                currentdoactive.Status = true;
            }
            else
                currentdoactive.Status = false;
            db.SaveChanges();


            return RedirectToAction("index");

        }

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