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
    public class CustomeProductsController : Controller
    {
        // GET: CustomeProducts

        DB_AlmaCmsEntities db;
        CustomeProductsRepository repProducts;
        CustomeProductImageRepository repImages;


        public CustomeProductsController()
        {
            db = new DB_AlmaCmsEntities();
            repImages = new CustomeProductImageRepository(db);
            repProducts = new CustomeProductsRepository(db);
        }
        public ActionResult Index(int id, string Title)
        {
            var CurrentProduct = repProducts.FindById(id);
            var ImageList = repImages.getByProductID(id).ToList();
            ViewBag.ImageList = ImageList;

            return View(CurrentProduct);
        }
    }
}