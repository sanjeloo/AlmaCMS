using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using AlmaCMS.Models;
using Dapper; // for using dapper  
using System.Data.SqlClient; 
namespace AlmaCMS.Controllers
{
    public class TagsController : Controller
    {
        // GET: Tags

 
        DB_AlmaCmsEntities db;
        SqlConnection conn;
        public TagsController()
        {
            db = new DB_AlmaCmsEntities();
            conn = new SqlConnection(db.Database.Connection.ConnectionString);
        }
        public ActionResult Index(string Title)
        {
            string TitleSearch = Title.Replace("-", " ");
            conn.Open();


            ViewBag.Title = Title.Replace("-", " ");


            var PagesList = conn.Query<Page>("select * from Pages where Keywords like N'%" + TitleSearch + "%'");
            conn.Close();
            var vmPagesList = new List<VMPages>();
            foreach (var item in PagesList )
            {
                vmPagesList.Add(item.toVMPages());
            }
            ViewBag.vmPagesList = vmPagesList;

            var ProjectsList = conn.Query<Project>("select * from Projects where Keywords like N'%" + TitleSearch + "%'");
            conn.Close();
            var vmProjectsList = new List<VMProjects>();
            foreach (var item in ProjectsList)
            {
                vmProjectsList.Add(item.toVMProject());
            }
            ViewBag.vmProjectsList = vmProjectsList;


            var NewsList = conn.Query<News>("select * from News where Keyword like N'%" + TitleSearch + "%'");
            conn.Close();
            var vmNewsList = new List<VMNews>();
            foreach (var item in NewsList)
            {
                vmNewsList.Add(item.ToVmNews());
            }
            ViewBag.vmNewsList = vmNewsList;


            var ArticleList = conn.Query<Article>("select * from Articles where Keyword like N'%" + TitleSearch + "%'");
            conn.Close();
            var vmArticleList = new List<VMArticle>();
            foreach (var item in ArticleList)
            {
                vmArticleList.Add(item.toVMArticle());
            }
            ViewBag.vmArticleList = vmArticleList;
            ViewBag.Title = Title.Replace("-", " ");
            return View();
        }
    }
}