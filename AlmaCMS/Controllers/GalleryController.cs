using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using AlmaCMS.Models;

namespace AlmaCMS.Controllers
{
    public class GalleryController : Controller
    {

        DB_AlmaCmsEntities db;
        AlbumeRepository repAlbume;
        GalleriesRepository repGallery;

        public GalleryController()
        {
            db = new DB_AlmaCmsEntities();
            repGallery = new GalleriesRepository(db);
            repAlbume = new AlbumeRepository(db);
        }
        // GET: Gallery
        public ActionResult Index()
        {

            var AlbumeList = repAlbume.GetAll().ToList();

            return View(AlbumeList);
        }

        public ActionResult list(int id,string Title)
        {

            ViewBag.AlbumeInfo = repAlbume.FindById(id);
            var ImagesList = repGallery.GetAllByAlbumeID(id).ToList();
            return View(ImagesList);
        }
    }
}