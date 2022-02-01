using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Repositories;
using AlmaCMS.Models;
using AlmaCMS.Helpers;
using AlmaCMS.ViewModels;

namespace AlmaCMS.Controllers
{
    public class SearchController : Controller
    {
        PagesRepository repPages;
        ArticleRepository repArticle;
        NewsRepository repNew;
        ProductsRepository repProducts;
        ProjectRepository repProject;
        CertificateRepository repCertificates;
        DB_AlmaCmsEntities db;
        CustomeProductsRepository repCustomProduct;
        public SearchController()
        {
            db = new DB_AlmaCmsEntities();
            repPages = new PagesRepository(db);
            repArticle = new ArticleRepository(db);
            repCertificates = new CertificateRepository(db);
            repNew = new NewsRepository(db);
            repProducts = new ProductsRepository(db);
            repProject = new ProjectRepository(db);
            repCustomProduct = new CustomeProductsRepository(db);
        }

        public ActionResult Index(string q)
        {

            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("index", "home");
            }
            ViewBag.SerachValue = q;

            var PagesList = repPages.Where(c => ( c.Title!=null && c.Title.Contains(q) )||( c.Keywords!=null && c.Keywords.Contains(q))).Where(c => c.PegeID != 0).ToList();
            ViewBag.PagesResult = PagesList;


            //var ArticleList = repArticle.Where(c =>( c.Title!=null && c.Title.Contains(q)) || ( c.Keyword !=null &&c.Keyword.Contains(q))).ToList();
            //ViewBag.ArticlesResult = ArticleList;

            var NewsList = repNew.Where(c =>c.Active==true && ( c.Title!=null &&c.Title.Contains(q)) ||( c.Keyword!=null && c.Keyword.Contains(q))).ToList();
            ViewBag.NewsList = NewsList;

            var ProductsList = repProducts.Where(c => (c.Title!=null && c.Title.Contains(q)) || (c.Keywords!=null && c.Keywords.Contains(q))).ToList();
            ViewBag.ProductsList = ProductsList;

             var CustomerProductList = repCustomProduct.Where(c => ( c.Title!=null && c.Title.Contains(q)) || (c.Keywords!=null && c.Keywords.Contains(q))).ToList();
            ViewBag.CustomerProductList = CustomerProductList;
            //var ProjectsList = repProject.Where(c =>( c.Title!=null && c.Title.Contains(q) )||(c.Keywords!=null && c.Keywords.Contains(q))).ToList();
            //ViewBag.ProjectsList = ProjectsList;


         


            return View();
        }
    }
}