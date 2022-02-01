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
namespace AlmaCMS.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [HasPermission("ManageLinks")]

    public class ManageLinksController : Controller
    {
        private DB_AlmaCmsEntities db;
        private LinkGroupRepository repLinkGroup;
        private LinksRepository repLinks;
        public ManageLinksController()
        {
            db = new DB_AlmaCmsEntities();
            repLinkGroup = new LinkGroupRepository(db);
            repLinks = new LinksRepository(db);
          
        }
        // GET: Manage/ManageLinks
        #region Index
        public ActionResult Index(int? page, int GroupID)
        {
            var LinkGroup = repLinkGroup.FindById(GroupID);
            if (LinkGroup != null)
                ViewBag.GroupTitle = LinkGroup.Title;

            var vmLinksList = new List<VMLinks>();
            foreach (var item in repLinks.getByGroupID(GroupID))
            {
                vmLinksList.Add(item.toVMlinks());
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.GroupID = GroupID;
            return View(vmLinksList.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = repLinks.FindById((int)id);
            if (link == null)
            {
                return HttpNotFound();
            }

            return View(link.toVMlinks());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMLinks vmLink)
        {
            if (ModelState.IsValid)
            {
                repLinks.Update(vmLink.toLink());
                Helpers.UserLogHelper.AddLog("پیوند ها", (User.Identity.Name), "درج", vmLink.Title);

                return RedirectToAction("Index", new { GroupID=vmLink.LinkGroupID });
            }
      
            return View(vmLink);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = repLinks.FindById((int)id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link.toVMlinks());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var CurrentItem = repLinks.FindById(id);
            int GroupID =(int) repLinks.FindById(id).LinkGroupID;
            repLinks.Delete(id);
            Helpers.UserLogHelper.AddLog("پیوند ها", (User.Identity.Name), "حذف", CurrentItem.Title);

            return RedirectToAction("Index", new { GroupID =GroupID});
        }
        #endregion

        #region Create
        public ActionResult Create(int GroupID)
        {
           
            VMLinks newLink = new VMLinks();
            ViewBag.GroupID = GroupID;
            newLink.LinkGroupID = GroupID;

            return View(newLink);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMLinks vmLinks)
        {
            if (ModelState.IsValid)
            {
                repLinks.Insert(vmLinks.toLink());
                Helpers.UserLogHelper.AddLog("پیوند ها", (User.Identity.Name), "درج", vmLinks.Title);

                return RedirectToAction("Index", new { GRoupID=vmLinks.LinkGroupID });
            }
          
            ;
            return View(vmLinks);
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