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

    public class ManageProductsGroupController : Controller
    {
        // GET: Manage/ManageProductsGroup
        public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private ProductsGroupRepository repProductsGroup;
        ProductsRepository RepProducts;
        public ManageProductsGroupController()
        {
            db = new DB_AlmaCmsEntities();
            repProductsGroup = new ProductsGroupRepository(db);
            RepProducts = new ProductsRepository(db);
        }

        // GET: Manage/ManageMenuItems
        public ActionResult Index(string searchString, int? page)
        {
            var vmGroupList = new List<VMProductGroup>();
            var groupList = repProductsGroup.GetAll().ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                groupList = groupList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }
            foreach (var item in groupList)
            {
                vmGroupList.Add(item.toVMProductGroup());
            }
            // groupList.ToList().Where(c=>c.ParentId==item.id)
            int pageSize = 300;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            return View(vmGroupList.ToPagedList(pageNumber, pageSize));

        }

        [HttpPost]
        public ActionResult GetProductGroupForGrid(int? groupid = 0)
        {
            var data = new List<dynamic>();
            var groupList = new List<ProductsGroup>();
            if (groupid > 0)
                groupList = repProductsGroup.Where(c => c.ParentId==groupid).ToList();
            else
                groupList = repProductsGroup.GetAll().ToList();

            foreach (var item in groupList)
            {
                data.Add(new
                {
                    Title = item.Title,
                    id = item.id,
                });
            }
            return Json(data);
        }
        [HttpPost]
        public ActionResult GetProductGroup()
        {
            var data = new List<dynamic>();
            var groupList = new List<ProductsGroup>();

            groupList = repProductsGroup.GetAll().ToList();
            foreach (var item in groupList)
            {
                data.Add(new { Name = item.Title, Id = item.id , ParentId = item.ParentId });
            }

            return Json(data);
        }
        
        #region Create
        
        public ActionResult Create(int? groupid = 0)
        {
            if (groupid>0)
                ViewBag.Groupid = groupid;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMProductGroup vmGroup)
        {

            if (ModelState.IsValid)
            {

                repProductsGroup.Insert(vmGroup.toProductsGroup());
                Helpers.UserLogHelper.AddLog("گروه محصولات", (User.Identity.Name), "درج", vmGroup.Title);

                return RedirectToAction("Index");
            }

            return View(vmGroup);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repProductsGroup.Where(c => c.id != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repProductsGroup.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentpage.toVMProductGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMProductGroup vmProductsgroup)
        {

            if (!ModelState.IsValid)
                return View(vmProductsgroup.toProductsGroup());




            repProductsGroup.Update(vmProductsgroup.toProductsGroup());
            Helpers.UserLogHelper.AddLog("گروه محصولات", (User.Identity.Name), "ویرایش", vmProductsgroup.Title);

            return RedirectToAction("Index");
        }
      
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repProductsGroup.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            if (RepProducts.getByGroupID(currentPage.id).ToList().Count > 0)
            {

                ViewBag.ErrorMessage = "امکان حذف گروه محصولات وجود ندارد.گروه دارای محصول می باشد";


            }
            return View(currentPage.toVMProductGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repProductsGroup.FindById(id);
            repProductsGroup.Delete(id);
            Helpers.UserLogHelper.AddLog("گروه محصولات", (User.Identity.Name), "حذف", CurrentItem.Title);

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