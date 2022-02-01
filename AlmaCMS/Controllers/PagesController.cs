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
    public class PagesController : Controller
    {
        DB_AlmaCmsEntities db;
        PagesRepository repPages;

        public PagesController()
        {
            db = new DB_AlmaCmsEntities();
            repPages = new PagesRepository(db);
        }
        // GET: Manage/Page
        public ActionResult Index(int id, string Title)
        {
            var currentPage = repPages.FindById(id);
            ViewBag.Title = currentPage.Title;
            ViewBag.Keywords = currentPage.Keywords;
            ViewBag.Description = currentPage.ShortDesc;
            ViewBag.TagList = currentPage.Keywords.Split(',');
            ViewBag.PageName = currentPage.PageName;
            return View(currentPage.toVMPages());
        }
    }
}