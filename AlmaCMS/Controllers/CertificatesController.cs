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
    public class CertificatesController : Controller
    {
        CertificateGroupRepository repCertificateGroup;
        CertificateRepository repCertificate;
        DB_AlmaCmsEntities db;

        public CertificatesController()
        {
            db = new DB_AlmaCmsEntities();
            repCertificate = new CertificateRepository(db);
            repCertificateGroup = new CertificateGroupRepository(db);
        }
        // GET: Certificates
        public ActionResult Index(int id, string Title)
        {
            var CertificateList = repCertificate.getByGroupID(id).ToList();
            List<VMCertificates> vmList = new List<VMCertificates>();
            foreach (var item in CertificateList)
            {
                vmList.Add(item.toVMCertificate());
            }
            var currentGroup = repCertificateGroup.FindById(id);


            ViewBag.Title = currentGroup.Title;
            ViewBag.Keywords = currentGroup.Keywords;
            ViewBag.Description = currentGroup.Descriotion;


            return View(vmList);
        }
        public ActionResult Details(int id, string Title)
        {
            var currentCertificate = repCertificate.FindById(id);
            var currentGroup = repCertificateGroup.FindById((int)currentCertificate.GroupID);
            ViewBag.GroupTitle = currentGroup.Title;
            ViewBag.GroupID = currentGroup.id;
            ViewBag.GroupTitleUrl = currentGroup.Title.Replace(" ", "-").Replace(".", "");





            ViewBag.TagList = currentCertificate.Keywords.Split(',');


            return View(currentCertificate.toVMCertificate());
        }
    }
}