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
    [HasPermission("ManageDiscountCode")]
    public class ManageDiscountCodeController : Controller
    {
        // GET: Manage/ManageDiscountCode
         public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private DiscountCodeRepository  repDiscount;
        //ArticleRepository repArticle;
        // GET: Manage/ManageAlbumes

        public ManageDiscountCodeController()
        {
            db = new DB_AlmaCmsEntities();
            repDiscount = new  DiscountCodeRepository(db);
            //repArticle = new ArticleRepository(db);
        }
        public ActionResult Index()
        {
          

            return View(repDiscount.GetAll().OrderByDescending(c=>c.id).ToList());

        }

        #region Create
        public ActionResult Create()
        {

            var newCode = new VMDiscountCode() {  DiscountPrice=0, CodeCount=0};

            return View(newCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMDiscountCode vmDiscount)
        {

            if (ModelState.IsValid)
            {


                for(int i=0 ;i <vmDiscount.CodeCount;i++)	{

                    repDiscount.Insert(new DiscountCode{
                     Code=Helpers.CreateHash.RNGCharacterMask(),
                      CreateDate=DateTime.Now,
                       Descriptions=vmDiscount.Descriptions,
                        Discount_price=vmDiscount.DiscountPrice,
                         ExpireDate=DateTime.Now.AddYears(5),
                          Used=false,

                    
                    });
		 
	}


             

                //Helpers.UserLogHelper.AddLog("آلبوم گالری", (User.Identity.Name), "درج", vmGroup.Title);

                return RedirectToAction("Index");
            }

            return View(vmDiscount);
        }
        #endregion


        

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repDiscount.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            //if (repAlbume.getByGroupID(currentPage.id).ToList().Count > 0)
            //{

            //    ViewBag.ErrorMessage = "امکان حذف گروه مقالات وجود ندارد.گروه دارای مقاله می باشد";


            //}
            return View(currentPage.toVMDiscountCode());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CuurentItem = repDiscount.FindById(id);
            repDiscount.Delete(id);
            //Helpers.UserLogHelper.AddLog("آلبوم گالری", (User.Identity.Name), "حذف", CuurentItem.Title);
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