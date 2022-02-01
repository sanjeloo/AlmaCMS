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
    public class ManageCustomeProductImageController : Controller
    {
        private DB_AlmaCmsEntities db;
        private CustomeProductsRepository repProduct;

        private CustomeProductImageRepository repProductImage;

        public ManageCustomeProductImageController()
        {
            db = new DB_AlmaCmsEntities();
            repProduct = new CustomeProductsRepository(db);
            repProductImage = new CustomeProductImageRepository(db);

        }
        // GET: Manage/ManageCustomeProductImage
        #region Index
        public ActionResult Index(int id)
        {
            var vmImagesList = new List<VMcustomeProductImage>();

            var ImagesList = repProductImage.getByProductID(id);


            foreach (var item in ImagesList)
            {
                vmImagesList.Add(item.TovmCustomeProductsImage());
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
            VMcustomeProductImage vmproductImage = new VMcustomeProductImage();

            ViewBag.GroupID = id;
            vmproductImage.ProductID = id;
            return View(vmproductImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMcustomeProductImage vmProductsImage)
        {
            if (ModelState.IsValid)
            {

                repProductImage.Insert(vmProductsImage.ToCustomeProductsImage());

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
            CustomeProsuctsImage productsImage = repProductImage.FindById((int)id);
            if (productsImage == null)
            {
                return HttpNotFound();
            }
            return View(productsImage.TovmCustomeProductsImage());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMcustomeProductImage vmProductsImage)
        {
            if (ModelState.IsValid)
            {
                repProductImage.Update(vmProductsImage.ToCustomeProductsImage());

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
            CustomeProsuctsImage productsImage = repProductImage.FindById((int)id);
            if (productsImage == null)
            {
                return HttpNotFound();
            }
            return View(productsImage.TovmCustomeProductsImage());
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