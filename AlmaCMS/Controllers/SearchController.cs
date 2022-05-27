using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Repositories;
using AlmaCMS.Models;
using AlmaCMS.Helpers;
using AlmaCMS.ViewModels;
using PagedList;
using AlmaCMS.DTOS;

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

        public ActionResult Index(string q)
        {

            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("index", "home");
            }
            ViewBag.SearchString = q;
            return View();
        }
        [HttpPost]
        public ActionResult QuickSearch(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Json("");
            var searchlist = text.Split(' ').ToList();
            var ProductsList = repProducts.Where(c => c.Visibility && (c.Title!=null && c.Title.Contains(text)))
                .Take(8).ToList();
            if (ProductsList.Count<8)
                ProductsList.AddRange(repProducts.Where(c => c.Visibility && c.Title!=null && searchlist.All(i => c.Title.Contains(i)))
                .Take(8 - ProductsList.Count).ToList());

            var GroupList = repProductsGroup.Where(c => c.Title!=null && searchlist.All(i => c.Title.Contains(i))).Take(2).ToList();
            var result = new List<dynamic>();
            foreach (var product in ProductsList)
            {
                result.Add(new { Title = product.Title, Id = product.id, TitleUrl = product.Title.toSlugify(), IsGroup = false });
            }
            foreach (var group in GroupList)
            {
                result.Add(new { Title = group.Title, Id = group.id, TitleUrl = "", IsGroup = true });
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult GetMoreResult(string q, int start, int pagesize)
        {
            if (string.IsNullOrEmpty(q))
                return Json("");

            var searchlist = q.Split(' ').ToList();


            List<ProductListItemDTO> data = repProducts.Where(c => c.Visibility && c.Title!=null && searchlist.All(i => c.Title.Contains(i))).OrderByDescending(p => p.id &p.Priority)
                .OrderByDescending(p => p.ExistStatus).Skip(start).Take(pagesize).Select(s => new ProductListItemDTO()
                {
                    id = s.id,
                    image = s.Image,
                    title = s.Title.toSlugify(),
                    normalTitle = s.Title
                }).ToList();
            start+=1;
            int totalcount = repProducts.Where(c => c.Visibility && c.Title!=null && searchlist.All(i => c.Title.Contains(i))).Count();
            var result = new ProductsListDTO()
            {
                data = data,
                pageSize = pagesize,
                start = start,
                totalCount =  totalcount
            };
            return Json(result);
        }
    }
}