using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Repositories;
using AlmaCMS.Models;
using AlmaCMS.Helpers;
using AlmaCMS.ViewModels;
using PagedList;

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
        ProductsGroupRepository repProductsGroup;

        CustomeProductsRepository repCustomProduct;
        public SearchController()
        {

            db = new DB_AlmaCmsEntities();
            repProductsGroup = new ProductsGroupRepository(db);
            repPages = new PagesRepository(db);
            repArticle = new ArticleRepository(db);
            repCertificates = new CertificateRepository(db);
            repNew = new NewsRepository(db);
            repProducts = new ProductsRepository(db);
            repProject = new ProjectRepository(db);
            repCustomProduct = new CustomeProductsRepository(db);
        }

        public ActionResult Index(string q , int? page)
        {

            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("index", "home");
            }
            ViewBag.SearchString = q;
            int pageNumber = (page ?? 1);
            //var PagesList = repPages.Where(c => (c.Title!=null && c.Title.Contains(q))||(c.Keywords!=null && c.Keywords.Contains(q))).Where(c => c.PegeID != 0).ToList();
            //ViewBag.PagesResult = PagesList;


            //var ArticleList = repArticle.Where(c =>( c.Title!=null && c.Title.Contains(q)) || ( c.Keyword !=null &&c.Keyword.Contains(q))).ToList();
            //ViewBag.ArticlesResult = ArticleList;

            //var NewsList = repNew.Where(c => c.Active==true && (c.Title!=null &&c.Title.Contains(q)) ||(c.Keyword!=null && c.Keyword.Contains(q))).ToList();
            //ViewBag.NewsList = NewsList;

            //var CustomerProductList = repCustomProduct.Where(c => (c.Title!=null && c.Title.Contains(q)) || (c.Keywords!=null && c.Keywords.Contains(q))).ToList();
            //ViewBag.CustomerProductList = CustomerProductList;
            //var ProjectsList = repProject.Where(c =>( c.Title!=null && c.Title.Contains(q) )||(c.Keywords!=null && c.Keywords.Contains(q))).ToList();
            //ViewBag.ProjectsList = ProjectsList;

            var ProductsList = repProducts.Where(c => (c.Title!=null && c.Title.Contains(q)) || (c.Keywords!=null && c.Keywords.Contains(q))).OrderByDescending(p=>p.id).ToList();
            List<VMProducts> vmList = new List<VMProducts>();
            foreach (var item in ProductsList)
            {
                vmList.Add(item.toVMProduct());
            }
            int pageSize = 16;
            ViewBag.pageNumber = pageNumber;
            return View(vmList.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult QuickSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Json("");
            var ProductsList = repProducts.Where(c => (c.Title!=null && c.Title.Contains(text)) || (c.Keywords!=null && c.Keywords.Contains(text))).Take(8).ToList();
            var GroupList = repProductsGroup.Where(c => c.Title.Contains(text)).Take(2).ToList();
            var result = new List<dynamic>();
            foreach (var product in ProductsList)
            {
                result.Add(new { Title = product.Title, Id = product.id, TitleUrl= product.Title.toSlugify(), IsGroup = false });
            }
            foreach (var group in GroupList)
            {
                result.Add(new { Title = group.Title, Id = group.id, TitleUrl ="", IsGroup = true });
            }
            return Json(result);
        }
    }
}