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
    [HasPermission("ManageProducts")]

    public class ManageProductsController : Controller
    { // GET: Manage/ManageArticles
        private DB_AlmaCmsEntities db;
        private ProductsRepository repProducts;
        private ProductsGroupRepository repProductsGroup;

        public static bool premission = false;

        public ManageProductsController()
        {
            db = new DB_AlmaCmsEntities();
            repProducts = new ProductsRepository(db);
            repProductsGroup = new ProductsGroupRepository(db);

        }
        #region Index
        public ActionResult Index(int id, string searchString, int? page)
        {
            //var vmProductsList = new List<VMProducts>();

            //var ProductsList = repProducts.getByGroupID(id);
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    ProductsList = ProductsList.Where(s => s.Title.Contains(searchString)).ToList();
            //    ViewBag.SearchString = searchString;
            //}


            //foreach (var item in ProductsList)
            //{
            //    vmProductsList.Add(item.toVMProduct());
            //}
            var ProductGroup = repProductsGroup.FindById(id);
            if (ProductGroup != null)
                ViewBag.GroupTitle = ProductGroup.Title;
            ViewBag.GroupID = ProductGroup.id;
            //int pageSize = 10;
            //int pageNumber = (page ?? 1);
            //ViewBag.pageNumber = pageNumber;
            return View();
        }
        #endregion

        #region Create
        public ActionResult Create(int id)
        {
            VMProducts vmProducts = new VMProducts();

            ViewBag.GroupID = id;
            vmProducts.GroupID = id;


            vmProducts.Price = 0;
            vmProducts.PriceBeforeDiscount = 0;
            return View(vmProducts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMProducts vmProducts)
        {


            if (ModelState.IsValid)
            {

                DateTime SpecialStartDate, SpecialEnDdate;
                SpecialEnDdate = DateTime.Parse("2018-01-01");
                SpecialStartDate = DateTime.Parse("2018-01-01");

                if (vmProducts.SpecialSaleStartDate != null)
                {
                    SpecialStartDate = (DateTime)vmProducts.SpecialSaleStartDate;
                }
                if (vmProducts.SpeciaSaleEndDate != null)
                {
                    SpecialEnDdate = (DateTime)vmProducts.SpeciaSaleEndDate;
                }

                vmProducts.SpecialSaleStartDate = SpecialStartDate;
                vmProducts.SpeciaSaleEndDate = SpecialEnDdate;
                repProducts.Insert(vmProducts.toPtoduct());

                vmProducts.VisitCount = 0;
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
            Product product = repProducts.FindById((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product.toVMProduct());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMProducts vmProduct)
        {
            if (ModelState.IsValid)
            {
                DateTime SpecialStartDate, SpecialEnDdate;
                SpecialEnDdate = DateTime.Parse("2018-01-01");
                SpecialStartDate = DateTime.Parse("2018-01-01");

                if (vmProduct.SpecialSaleStartDate != null)
                {
                    SpecialStartDate = (DateTime)vmProduct.SpecialSaleStartDate;
                }
                if (vmProduct.SpeciaSaleEndDate != null)
                {
                    SpecialEnDdate = (DateTime)vmProduct.SpeciaSaleEndDate;
                }
                repProducts.Update(vmProduct.toPtoduct());
                Helpers.UserLogHelper.AddLog("محصولات", (User.Identity.Name), "ویرایش", vmProduct.Title);

                return RedirectToAction("Index", new { id = vmProduct.GroupID });
            }
            return View(vmProduct);
        }

        [HttpPost]
        public ActionResult UpdateProductGroup(int groupId, int proudctId)
        {
            Product product = repProducts.FindById(proudctId);
            product.GroupID = groupId;
            repProducts.Update(product);
            Helpers.UserLogHelper.AddLog("محصولات", (User.Identity.Name), "ویرایش", product.Title);
            return Json("ok");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product prduct = repProducts.FindById((int)id);
            if (prduct == null)
            {
                return HttpNotFound();
            }
            return View(prduct.toVMProduct());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repProducts.FindById(id);

            int GroupID = (int)repProducts.FindById(id).GroupID;
            repProducts.Delete(id);
            Helpers.UserLogHelper.AddLog("محصولات", (User.Identity.Name), "حذف", CurrentItem.Title);

            return RedirectToAction("Index", new { id = GroupID });
        }
        #endregion

        [HttpPost]
        public ActionResult ProductDataTable(int id = 0)
        {
            // todo if id was not pass or was 0 return

            //int pagenumber = Int32.Parse( Request.Form["draw"].ToString());
            int start = Int32.Parse(Request.Form["start"].ToString());
            string search = Request.Form["search[value]"].ToString();
            int length = Int32.Parse(Request.Form["length"].ToString());

            //================== todo for search ===================//
            dynamic result = new List<dynamic>();
            var ProductsList = new List<Product>();
            int count = 0;
            if (!String.IsNullOrEmpty(search))
            {
                ProductsList = repProducts.Where(p => p.GroupID==id && p.Title.Contains(search)).Skip(start).Take(length).ToList();
                count = repProducts.Where(p => p.GroupID==id && p.Title.Contains(search)).OrderByDescending(p => p.id).Count();

            }
            else
            {
                ProductsList = repProducts.Where(p => p.GroupID==id).OrderByDescending(p => p.id).Skip(start).Take(length).ToList();
                count = repProducts.Where(p => p.GroupID==id).OrderByDescending(p => p.id).Count();

            }
            int i = 1;
            foreach (var item in ProductsList)
            {
                result.Add(new
                {
                    id = item.id,
                    radif = start+i,
                    title = item.Title,
                    link = "",
                    operation = ""
                });
                i++;
            }

            return Json(new { recordsTotal = count, recordsFiltered = count, data = result });
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