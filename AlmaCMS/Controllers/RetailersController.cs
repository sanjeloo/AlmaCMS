using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;

using PagedList;
using PagedList.Mvc;
using System.Web.Routing;
using System.Web.Security;

using CaptchaMvc;
using CaptchaMvc.HtmlHelpers;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
namespace AlmaCMS.Controllers
{
    
    public class RetailersController : Controller
    {
        private DB_AlmaCmsEntities db;
        private RetailerRepository repRetailers;
        // GET: Retailers
        private SecureArticleRepository repSArticles;
        public RetailersController()
        {
            db = new DB_AlmaCmsEntities();
            repRetailers = new RetailerRepository(db);
            repSArticles = new SecureArticleRepository(db);
        }
        public ActionResult Index()
        {
            return View();
        }

      

        [HttpPost]
        public ActionResult Index(VMRetailerLogin vmlogin)
        {


            if (ModelState.IsValid)
            {
                var currentuser = repRetailers.getByUserName(vmlogin.UserName);
                if(currentuser!=null)
                {
                    if(currentuser.Pass==vmlogin.Pass)
                    {
                        Session["RetailerLogin"] = currentuser.id;
                        return RedirectToAction("posts");
                    }
                    else
                    {
                        ViewBag.Msg = "اطلاعات وارد شده صحیح نمی باشد";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Msg = "اطلاعات وارد شده صحیح نمی باشد";
                    return View();
                }
            }
            else
            {
                return View(vmlogin);
            }
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(VMRegisterRetailer vmRegister)
        {
        
            if(ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if (!this.IsCaptchaValid("*"))
                    {
                        ViewBag.CaptchaMsg = "کد امنیتی وارد شده صحیح نمی باشد";
                        return View(vmRegister);
                    }
                }

                string RequestEmail = (System.Web.Configuration.WebConfigurationManager.AppSettings["RetailerRequest"].ToString());

                string text = System.IO.File.ReadAllText(Server.MapPath("~") + "\\Content\\Email-temp\\RetailerRequest.html");

                text = text.Replace("'%Name%'", vmRegister.Name);
                if( Helpers.CustomEmail.sendemail("درخواست نمایندگی : " + vmRegister.Name + " " + vmRegister.Companyfriendship, RequestEmail, text.ToString()))
                {
                    ModelState.Clear();
                    ViewBag.EmailState = "1";
                    return View();
                }
                else
                {
                    ViewBag.EmailState = "0";
                    return View(vmRegister);
                }
               
                
            }
            else
            {
                return View(vmRegister);
            }
            
        }
        public ActionResult Posts()
        {
            try
            {
                var currentuser = Session["RetailerLogin"].ToString();
                List<VMSecureArticle> vmList = new List<VMSecureArticle>();

                var ArticleList = repSArticles.GetAll().OrderByDescending(c => c.id).ToList();
                foreach(var item in ArticleList)
                {
                    vmList.Add(item.toVMScureArticles());
                }
                return View(vmList);
            }
                           
            catch(Exception ex)
            {
                return RedirectToAction("Index");
            }
            
            
           
        }

        public ActionResult Details(int id, string Title)
        {
              try
            {
            var currentuser = Session["RetailerLogin"].ToString();
            var currentArticles = repSArticles.FindById(id);

            return View(currentArticles.toVMScureArticles());
            }

              catch (Exception ex)
              {
                  return RedirectToAction("Index");
              }
        }
    }
}