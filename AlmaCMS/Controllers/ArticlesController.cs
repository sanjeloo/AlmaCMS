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
    public class ArticlesController : Controller
    {
        // GET: Articles
         ArticleRepository repArticles;
        ArticleGroupRepositry repArticlesGroup;
        DB_AlmaCmsEntities db;

        public ArticlesController()
        {
            db = new DB_AlmaCmsEntities();
            repArticles = new ArticleRepository(db);
            repArticlesGroup = new ArticleGroupRepositry(db);
        }
        // GET: Articles
        public ActionResult Index(int id,string Title)
        {
            var ArticlesList = repArticles.getByGroupID(id).Where(c => c.Active == true).ToList();
            List<VMArticle> vmList = new List<VMArticle>();
            foreach (var item in ArticlesList)
            {
                vmList.Add(item.toVMArticle());
            }
            var currentGroup = repArticlesGroup.FindById(id);


            ViewBag.Title = currentGroup.Title;
            ViewBag.Keywords = currentGroup.Keywords;
            ViewBag.Description = currentGroup.Keywords;


            return View(vmList);
        }

        public ActionResult Details(int id, string Title)
        {
            var currentArticles = repArticles.FindById(id);
            var currentGroup = repArticlesGroup.FindById((int)currentArticles.ArticleGroupID);
            ViewBag.GroupTitle = currentGroup.Title;
            ViewBag.GroupID = currentGroup.id;
            ViewBag.GroupTitleUrl = currentGroup.Title.Replace(" ", "-").Replace(".", "");





            ViewBag.TagList = currentArticles.Keyword.Split(',');


            return View(currentArticles.toVMArticle());
        }
    }
}