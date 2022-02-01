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
    [HasPermission("ManageCustomeProducts")]
    public class ManageCustomeProductsController : Controller
    {
        private DB_AlmaCmsEntities db;
        private CustomeProductsRepository repProducts;
        private CustomeProductGroupRepository repProductsGroup;
        public static bool premission = false;
        // GET: Manage/ManageCustomePrioducts
        public ManageCustomeProductsController()
        {
            db = new DB_AlmaCmsEntities();
            repProducts = new CustomeProductsRepository(db);
            repProductsGroup = new CustomeProductGroupRepository(db);
           
        }
        #region Index
        public ActionResult Index(int id, string searchString, int? page)
        {
            var vmProductsList = new List<VMCustomeProduct>();

            var ProductsList = repProducts.getByGroupID(id);
            if (!String.IsNullOrEmpty(searchString))
            {
                ProductsList = ProductsList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }


            foreach (var item in ProductsList)
            {
                vmProductsList.Add(item.toVMCustomeProduct());
            }
            var ProductGroup = repProductsGroup.FindById(id);
            if (ProductGroup != null)
                ViewBag.GroupTitle = ProductGroup.Title;
            ViewBag.GroupID = ProductGroup.id;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            return View(vmProductsList.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create(int id)
        {
            VMCustomeProduct vmProducts = new VMCustomeProduct();
            
            ViewBag.GroupID = id;
            vmProducts.GroupID = id;


            return View(vmProducts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( VMCustomeProduct vmProducts)
        {


            if (ModelState.IsValid)
            {

                DateTime SpecialStartDate, SpecialEnDdate;

                repProducts.Insert(vmProducts.tocustomePtoduct());
              
             
                Helpers.UserLogHelper.AddLog("محصولات", (User.Identity.Name), "درج", vmProducts.Title);

                return RedirectToAction("Index", new { id = vmProducts.GroupID });
            }

            return View(vmProducts);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomProduct product = repProducts.FindById((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product.toVMCustomeProduct());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( VMCustomeProduct vmProduct)
        {
            if (ModelState.IsValid)
            {
       

               
                repProducts.Update(vmProduct.tocustomePtoduct());
                Helpers.UserLogHelper.AddLog("محصولات", (User.Identity.Name), "ویرایش", vmProduct.Title);

                return RedirectToAction("Index", new { id = vmProduct.GroupID });
            }
            return View(vmProduct);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomProduct prduct = repProducts.FindById((int)id);
            if (prduct == null)
            {
                return HttpNotFound();
            }
            return View(prduct.toVMCustomeProduct());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repProducts.FindById(id);

            int GroupID = (int)repProducts.FindById(id).GroupId;
            repProducts.Delete(id);
            Helpers.UserLogHelper.AddLog("محصولات", (User.Identity.Name), "حذف", CurrentItem.Title);

            return RedirectToAction("Index", new { id = GroupID });
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