using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using AlmaCMS.Models;
using PagedList;
using PagedList.Mvc;
namespace AlmaCMS.Controllers
{
    public class NewsController : Controller
    {
        NewsRepository repNews;
        NewsGroupRepository repNewsGroup;
        DB_AlmaCmsEntities db;

        public NewsController()
        {
            db = new DB_AlmaCmsEntities();
            repNews = new NewsRepository(db);
            repNewsGroup = new NewsGroupRepository(db);
        }
        // GET: News
        public ActionResult Index(int id,string Title, int? page)
        {
            var NewsList = repNews.getByGroupID(id).Where(c => c.Active == true).ToList();
            List<VMNews> vmList = new List<VMNews>();
            foreach (var item in NewsList)
            {
                vmList.Add(item.ToVmNews());
            }
            var currentGroup = repNewsGroup.FindById(id);


            ViewBag.Title = currentGroup.Title;
            ViewBag.Keywords = currentGroup.Keywords;
            ViewBag.Description = currentGroup.Title + currentGroup.Keywords;
            ViewBag.GroupInfo = currentGroup;

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            return View(vmList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id, string Title)
        {
            var currentNews = repNews.FindById(id);
            var currentGroup = repNewsGroup.FindById((int)currentNews.NewsGroupID);
            ViewBag.GroupTitle = currentGroup.Title;
            ViewBag.GroupID = currentGroup.id;
            ViewBag.GroupTitleUrl = currentGroup.Title.Replace(" ", "-").Replace(".", "");





            ViewBag.TagList = currentNews.Keyword.Split(',');
            repNews.AddVisitCount(currentNews.id);




            ViewBag.GroupInfo = currentGroup;



            return View(currentNews.ToVmNews());

           
        }
    }
}