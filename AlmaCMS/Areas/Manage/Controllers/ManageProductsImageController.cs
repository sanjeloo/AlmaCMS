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


    public class ManageProductsImageController : Controller
    {
        private DB_AlmaCmsEntities db;
        private ProductsRepository repProduct;

        private ProductsImageRepository repProductImage;

        // GET: Manage/ManageProductsImage

        public ManageProductsImageController()
        {
            db = new DB_AlmaCmsEntities();
            repProduct = new ProductsRepository(db);

            repProductImage = new ProductsImageRepository(db);


        }
        // GET: Manage/ManageProductsImage

        #region Index
        public ActionResult Index(int id)
        {
            var vmImagesList = new List<VMProductsImage>();

            var ImagesList = repProductImage.getByProductID(id);


            foreach (var item in ImagesList)
            {
                vmImagesList.Add(item.TovmProductsImage());
            }
            var ImagesGroup = repProduct.FindById(id);
            if (ImagesGroup != null)
                ViewBag.GroupTitle = ImagesGroup.Title;
            ViewBag.GroupID = ImagesGroup.id;

            ViewBag.currentProduct = ImagesGroup;

            return View(vmImagesList.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create(int id)
        {
            VMProductsImage vmproductImage = new VMProductsImage();

            ViewBag.GroupID = id;
            vmproductImage.ProductID = id;
            return View(vmproductImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMProductsImage vmProductsImage)
        {
            if (ModelState.IsValid)
            {

                repProductImage.Insert(vmProductsImage.ToProductsImage());

                return RedirectToAction("Index", new { id = vmProductsImage.ProductID });
            }

            return View(vmProductsImage);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsImage productsImage = repProductImage.FindById((int)id);
            if (productsImage == null)
            {
                return HttpNotFound();
            }
            return View(productsImage.TovmProductsImage());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMProductsImage vmProductsImage)
        {
            if (ModelState.IsValid)
            {
                repProductImage.Update(vmProductsImage.ToProductsImage());

                return RedirectToAction("Index", new { id = vmProductsImage.ProductID });
            }
            return View(vmProductsImage);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsImage productsImage = repProductImage.FindById((int)id);
            if (productsImage == null)
            {
                return HttpNotFound();
            }
            return View(productsImage.TovmProductsImage());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repProductImage.FindById(id);

            int ProductID = (int)repProductImage.FindById(id).ProductID;
            repProductImage.Delete(id);

            return RedirectToAction("Index", new { id = ProductID });
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