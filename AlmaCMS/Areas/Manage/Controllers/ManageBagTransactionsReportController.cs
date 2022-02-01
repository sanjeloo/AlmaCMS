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
using System.IO;
using Newtonsoft.Json;

namespace AlmaCMS.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [HasPermission("ManageTasks")]
    public class ManageBagTransactionsReportController : Controller
    {
        // GET: Manage/ManageBagTransactionsReport
        public ActionResult Index()
        {
            DB_AlmaCmsEntities db = new DB_AlmaCmsEntities();
            return View(db.VWBagTransactions.OrderByDescending(c=>c.id).ToList());
        }
    }
}