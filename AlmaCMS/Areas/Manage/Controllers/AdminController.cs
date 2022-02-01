using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Helpers;
using AlmaCMS.Repositories;
using System.Net;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using AlmaCMS.Models;
namespace AlmaCMS.Areas.Manage.Controllers
{
     [Authorize]
    public class AdminController : Controller
    {
        public static bool locked = false;
         

         
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        // GET: Manage/Admin

        DB_AlmaCmsEntities db;
        ProductsRepository repProducts;
        TasksRepository repTasks;
        ProductReportRepository repProductReport;
        ShortMessageRepository repShortMessage;
        CustomeProductsRepository repCustomeProducts;
        AspUserRepository repUser;
         public AdminController()
        {
            db = new Models.DB_AlmaCmsEntities();
            repProductReport = new ProductReportRepository(db);
            repProducts = new ProductsRepository(db);
            repTasks = new TasksRepository(db);
            repShortMessage = new ShortMessageRepository(db);
            repCustomeProducts = new CustomeProductsRepository(db);

            repUser = new AspUserRepository(db);
        }
        public ActionResult Dashboard()
        {

            ViewBag.CustomeProducList = repCustomeProducts.GetAll().ToList();
            ViewBag.TasksList = repTasks.GetDapperTaskList();
            ViewBag.Products = repProducts.GetAll();
            ViewBag.LastShortMessages = repShortMessage.GetAll().OrderByDescending(c => c.id).Take(10).ToList();
            ViewBag.MemberList=repUser.GetAll().Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Member")).ToList();
            ViewBag.ExpertList = repUser.GetAll().Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Expert")).ToList();

            ViewBag.ProductReportList = repProductReport.GetDapperReportList().ToList();
            if (locked)
                ViewBag.suspend = "true";
            return View();
         
        }

        public ActionResult noaccess()
        {
            if (locked)
                ViewBag.suspend = "true";
            return View();

        }

            public ActionResult LogOut()
            {
                
                AuthenticationManager.SignOut();
                //FormsAuthentication.SignOut();
                //ManageNewsController.premission = false;
                //ManagePagesController.premission = false;
                //ManageManagersController.premission = false;
                //ManageCertificatesController.premission = false;
                //ManageSiteMessagesController.premission = false;
                //ManageProjectFeaturesController.premission = false;
                //ManageProjectsController.premission = false;
                //UsersController.premission = false;
                
                return RedirectToAction("Login", "Account", new { area = "" });
            }
    }
}