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
    [Authorize(Roles="Admin")]
  [HasPermission("ManageImageGallery")]

    public class ManageGalleriesController : Controller
    {
        private DB_AlmaCmsEntities db;
        private GalleriesRepository repGalleries;
        private AlbumeRepository repAlbums;
        private MediaTypesRepository repMediaType;
        public ManageGalleriesController()
        {
            db = new DB_AlmaCmsEntities();
            repGalleries = new GalleriesRepository(db);
            repAlbums = new AlbumeRepository(db);
            repMediaType = new MediaTypesRepository(db);
        }

        #region Index
        public ActionResult Index(int? page, int AlbumId)
        {
            var GalleryGroup = repAlbums.FindById(AlbumId);
            if (GalleryGroup != null)
                ViewBag.GroupTitle = GalleryGroup.Title;

            var galleriesList = repGalleries.GetAll().Where(e => e.AlbumId == AlbumId);

            var vmGalleriesList = new List<VMGallery>();
            foreach (var item in repGalleries.GetAllByAlbumeID(AlbumId))
            {
                vmGalleriesList.Add(item.ToVmGallery());
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.AlbumID = AlbumId;
            return View(vmGalleriesList.ToPagedList(pageNumber, pageSize));
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = repGalleries.FindById((int)id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            ViewBag.MediaTypeId = repMediaType.GetAll().ToList();
            return View(gallery.ToVmGallery());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMGallery vmGallery)
        {
            if (ModelState.IsValid)
            {
                repGalleries.Update(vmGallery.ToGallery());
                ViewBag.MediaTypeId = repMediaType.GetAll().ToList();
                return RedirectToAction("Index", new { vmGallery.AlbumId });
            }
            ViewBag.MediaTypeId = repMediaType.GetAll().ToList();
            return View(vmGallery);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = repGalleries.FindById((int)id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery.ToVmGallery());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int AlbumId = repGalleries.FindById(id).AlbumId;
            repGalleries.Delete(id);
            return RedirectToAction("Index", new { AlbumId });
        }
        #endregion

        #region Create
        public ActionResult Create(int AlbumId)
        {
            ViewBag.AlbumId = new SelectList(repAlbums.GetAll(), "id", "Title");
            ViewBag.MediaTypeId = repMediaType.GetAll().ToList();
            VMGallery newgallery = new VMGallery();
            ViewBag.AlbumeID = AlbumId;
            newgallery.AlbumId = AlbumId;

            return View(newgallery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMGallery vmGallery)
        {
            if (ModelState.IsValid)
            {
                repGalleries.Insert(vmGallery.ToGallery());
                return RedirectToAction("Index", new { vmGallery.AlbumId });
            }
            ViewBag.MediaTypeId = repMediaType.GetAll().ToList();
            ViewBag.AlbumId = new SelectList(repAlbums.GetAll(), "id", "Title", vmGallery.AlbumId);
            return View(vmGallery);
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