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

    [Authorize(Roles="Admin")]


    [HasPermission("ManageImageGallery")]
    public class ManageAlbumesController : Controller
    {
        public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private AlbumeRepository repAlbume;
        //ArticleRepository repArticle;
        // GET: Manage/ManageAlbumes

        public ManageAlbumesController()
        {
            db = new DB_AlmaCmsEntities();
            repAlbume = new AlbumeRepository(db);
            //repArticle = new ArticleRepository(db);
        }
        public ActionResult Index()
        {
            var vmGroupList = new List<VMAlbums>();
            var groupList = repAlbume.GetAll().ToList();

            foreach (var item in groupList)
            {
                vmGroupList.Add(item.toVMAlbume());
            }

            return View(vmGroupList.ToList());

        }

        #region Create
        public ActionResult Create()
        {

            var newAlbume = new VMAlbums();

            return View(newAlbume);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMAlbums vmGroup)
        {

            if (ModelState.IsValid)
            {



                repAlbume.Insert(vmGroup.toAlbume());

                Helpers.UserLogHelper.AddLog("آلبوم گالری", (User.Identity.Name), "درج", vmGroup.Title);

                return RedirectToAction("Index");
            }

            return View(vmGroup);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            ViewBag.ParentPages = repAlbume.Where(c => c.id != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repAlbume.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentpage.toVMAlbume());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMAlbums vmAlbume)
        {

            if (!ModelState.IsValid)
                Helpers.UserLogHelper.AddLog("آلبوم گالری", (User.Identity.Name), "ویرایش", vmAlbume.Title);
                return View(vmAlbume);

            var currentPAge = repAlbume.FindById(vmAlbume.id);
            currentPAge.Comments = vmAlbume.Comments;
            currentPAge.Keywords = vmAlbume.Keywords;
            currentPAge.Title = vmAlbume.Title;
            //currentPAge.Name = vmMenu.Name;


            repAlbume.Update(currentPAge);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repAlbume.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            //if (repAlbume.getByGroupID(currentPage.id).ToList().Count > 0)
            //{

            //    ViewBag.ErrorMessage = "امکان حذف گروه مقالات وجود ندارد.گروه دارای مقاله می باشد";


            //}
            return View(currentPage.toVMAlbume());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CuurentItem = repAlbume.FindById(id);
            repAlbume.Delete(id);
            Helpers.UserLogHelper.AddLog("آلبوم گالری", (User.Identity.Name), "حذف", CuurentItem.Title);
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