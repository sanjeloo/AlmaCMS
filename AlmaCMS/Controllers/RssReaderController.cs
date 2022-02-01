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

namespace AlmaCMS.Controllers
{
    public class RssReaderController : Controller
    {
        private DB_AlmaCmsEntities db;
        private RSSReaderRepository repRSS;
        

        public RssReaderController()
        {

            db = new DB_AlmaCmsEntities();
            repRSS = new RSSReaderRepository(db);

        }
        // GET: RssReader
        public ActionResult Index()
        {
            var rssList = repRSS.GetAll();
            List<VMRSSReader> vmList = new List<VMRSSReader>();
            foreach(var item in rssList)
            {
                vmList.Add(item.toVMRssReader());
            }
            return View(vmList);
        }

        //نمایش اخبار فید
        public ActionResult RssReader(string url)
        {
            return View(Helpers.RssReaderHelper.SiteRssReader.GetRssFeed(url));
        }
    }
}