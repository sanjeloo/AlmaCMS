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
    public class ProjectsController : Controller
    {
           ProjectRepository repProjects;
        ProjectGroupRepository repProjectsGroup;
        DB_AlmaCmsEntities db;

        public ProjectsController()
        {
            db = new DB_AlmaCmsEntities();
            repProjects = new ProjectRepository(db);
            repProjectsGroup = new ProjectGroupRepository(db);
        }
        // GET: Projects
        public ActionResult Index(int id,string Title)
        {
            var ProjectsList = repProjects.getByGroupID(id).ToList();
            List<VMProjects> vmList = new List<VMProjects>();
            foreach (var item in ProjectsList)
            {
                vmList.Add(item.toVMProject());
            }
            var currentGroup = repProjectsGroup.FindById(id);


            ViewBag.Title = currentGroup.Title;
            ViewBag.Keywords = currentGroup.Keywords;
            ViewBag.Description = currentGroup.Title + currentGroup.Keywords;


            return View(vmList);
        }

        public ActionResult Details(int id, string Title)
        {
            var currentProjects = repProjects.FindById(id);
            var currentGroup = repProjectsGroup.FindById((int)currentProjects.GroupID);
            ViewBag.GroupTitle = currentGroup.Title;
            ViewBag.GroupID = currentGroup.id;
            ViewBag.GroupTitleUrl = currentGroup.Title.Replace(" ", "-").Replace(".", "");





            ViewBag.TagList = currentProjects.Keywords.Split(',');


            return View(currentProjects.toVMProject());
        }
    }
}