using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using AlmaCMS.Models;
using System.Web.Hosting;
namespace AlmaCMS.Controllers
{
    public class HomeController : Controller
    {

        private DB_AlmaCmsEntities db;
        private SliderRepository repSlider;
        private ProductsRepository repProduct;
        private LinksRepository repLinks;
        private NewsRepository repNews;
        public HomeController()
        {
            db = new DB_AlmaCmsEntities();
            repSlider = new SliderRepository(db);
            repLinks = new LinksRepository(db);
            repProduct = new ProductsRepository(db);

            repNews = new NewsRepository(db);
        }
        [HttpGet]
        public ActionResult Index()
        {
            //HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => new Helpers.Worker().StartProcessing(cancellationToken));  
            //SMSHelper.SendSMS("09195305100", "", "test sms");

            ViewBag.Sliders = repSlider.Where(c => c.Active == true).OrderByDescending(c => c.Priority).ToList();
            ViewBag.LastProsucts = repProduct.GetAll().OrderByDescending(c => c.id).Take(12).ToList();
            ViewBag.HomeProducts = repLinks.getByGroupID(7).OrderByDescending(c=>c.id).ToList();

            ViewBag.LastNews = repNews.Where(c => c.Active == true).OrderByDescending(c => c.id).Take(8).ToList();

            ViewBag.HomePAgeInfo = repLinks.getByGroupID(12).ToList();

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
