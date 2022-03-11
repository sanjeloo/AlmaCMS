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
    public class ProductsController : Controller
    {
              ProductsRepository repProducts;
        ProductsGroupRepository repProductsGroup;
        DB_AlmaCmsEntities db;

        ProductsImageRepository repImages;
        ProductReviewRepository repPReview;
        public ProductsController()
        {
        
            db = new DB_AlmaCmsEntities();
            repProducts = new ProductsRepository(db);
            repProductsGroup = new ProductsGroupRepository(db);
            repImages = new ProductsImageRepository(db);
            repPReview = new ProductReviewRepository(db);
        }
        // GET: Productss
        public ActionResult Index(int id,string Title, int? page)
        {
            var vMProducts = new List<VMProductGroup>();
            var ProductsList = repProducts.getByGroupID(id).ToList();
            List<VMProducts> vmList = new List<VMProducts>();
            foreach (var item in ProductsList)
            {
                vmList.Add(item.toVMProduct());
            }
           

            var currentGroup = repProductsGroup.FindById(id);


            ViewBag.Title = currentGroup.Title;
            ViewBag.Keywords = currentGroup.Keywords;
            ViewBag.Description = currentGroup.Title + currentGroup.Keywords;

            ViewBag.GroupInfo = currentGroup;

            int pageSize = 16;
            //=============added by amin =================//
            int pageNumber = (page ?? 1);
            ViewBag.pageNumber = pageNumber;
            return View(vmList.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult GetProductGroup()
        {
            var data = new List<dynamic>();
            var groupList = new List<ProductsGroup>();

            groupList = repProductsGroup.GetAll().OrderBy(p=>p.Priority).ToList();
            foreach (var item in groupList)
            {
                data.Add(new { Name = item.Title, Id = item.id, ParentId = item.ParentId });
            }

            return Json(data);
        }
        public ActionResult Details(int id, string Title)
        {
            var currentProduct = repProducts.FindById(id);
            var currentGroup = repProductsGroup.FindById((int)currentProduct.GroupID);
            ViewBag.GroupTitle = currentGroup.Title;
            ViewBag.GroupID = currentGroup.id;
            ViewBag.GroupTitleUrl = currentGroup.Title.Replace(" ", "-").Replace(".", "");

            ViewBag.ReviewList = repPReview.getByProductID(id).Where(c => c.Status == true).ToList();
            ViewBag.GroupInfo = currentGroup;


            ViewBag.TagList = currentProduct.Keywords.Split(',');


            ViewBag.ImageList = repImages.getByProductID(id).ToList();
            repProducts.AddVisitCount(id);
            return View(currentProduct.toVMProduct());
        }
        public ActionResult AddComments(int ProductID, string Name, string Email, string Title, string Comment, int Rate)
        {

            try
            {
                var newcomment = new ProductReview()
                {
                    Comment = Comment,
                    Title = Title,
                    Rate = Rate,
                    Name = Name,
                    Status = false,
                    DateInsert = DateTime.Now,
                    productid = ProductID,
                    Email = Email


                };

                repPReview.Insert(newcomment);
                return Json("1");
            }
            catch (Exception ex)
            {
                return Json(-1);
            }


        }
        //public ActionResult AddComments(int ProductID, string Name, string Email, string Title, string Comment, int Rate)
        //{

        //    try
        //    {
        //        var newcomment = new ProductReview()
        //        {
        //            Comment = Comment,
        //            Title = Title,
        //            Rate = Rate,
        //            Name = Name,
        //            Status = false,
        //            DateInsert = DateTime.Now,
        //            ProductID = ProductID,
        //            Email = Email


        //        };

        //        repPReview.Insert(newcomment);
        //        return Json("1");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(-1);
        //    }


        //}
    }
}