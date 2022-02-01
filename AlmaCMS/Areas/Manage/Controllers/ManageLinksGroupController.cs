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
   [HasPermission("ManageLinks")]
    public class ManageLinksGroupController : Controller
    {
        // GET: Manage/ManageLinksGroup
          public static bool premission = false;
        private DB_AlmaCmsEntities db;
        private LinkGroupRepository repLinkGroup;
        LinksRepository repLinks;
        public ManageLinksGroupController()
        {
            db = new DB_AlmaCmsEntities();
            repLinkGroup = new LinkGroupRepository(db);
            repLinks = new LinksRepository(db);
        }

        // GET: Manage/ManageMenuItems
        public ActionResult Index(string searchString, int? page)
        {
            var vmGroupList = new List<VMLinkGroup>();
            var groupList = repLinkGroup.GetAll().ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                groupList = groupList.Where(s => s.Title.Contains(searchString)).ToList();
                ViewBag.SearchString = searchString;
            }
            foreach (var item in groupList)
            {
                vmGroupList.Add(item.tovmLinkGroup());
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            return View(vmGroupList.ToPagedList(pageNumber, pageSize));

        }

        #region Create
        public ActionResult Create()
        {




            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMLinkGroup vmGroup)
        {
         
            if (ModelState.IsValid)
            {



                repLinkGroup.Insert(vmGroup.toLinkGroup());
                Helpers.UserLogHelper.AddLog("گروه پیوند ها", (User.Identity.Name), "درج", vmGroup.Title);

                return RedirectToAction("Index");
            }

            return View(vmGroup);
        }
        #endregion


        #region Edit
        public ActionResult Edit(int? id)
        {
            //ViewBag.ParentPages = repLinkGroup.Where(c => c.id != id).ToList();
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentpage = repLinkGroup.FindById((int)id);
            if (currentpage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(currentpage.tovmLinkGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMLinkGroup vmLinkGroup)
        {
          
            if (!ModelState.IsValid)
                return View(vmLinkGroup);

            var currentPAge = repLinkGroup.FindById(vmLinkGroup.id);
            currentPAge.Comments = vmLinkGroup.Comments;
            currentPAge.Title = vmLinkGroup.Title;
            //currentPAge.Name = vmMenu.Name;
        

            repLinkGroup.Update(currentPAge);
            Helpers.UserLogHelper.AddLog("گروه پیوند ها", (User.Identity.Name), "ویرایش", vmLinkGroup.Title);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repLinkGroup.FindById((int)id);
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);


            if (repLinks.getByGroupID(currentPage.id).ToList().Count > 0)
            {

                ViewBag.ErrorMessage = "امکان حذف گروه لینک ها وجود ندارد.گروه دارای لینک می باشد";


            }
            return View(currentPage.tovmLinkGroup());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var CurrentItem = repLinkGroup.FindById(id);
            repLinkGroup.Delete(id);
            Helpers.UserLogHelper.AddLog("گروه پیوند ها", (User.Identity.Name), "حذف", CurrentItem.Title);

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