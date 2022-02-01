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

namespace AlmaCMS.Areas.Manage.Controllers
{
         [Authorize(Roles = "Admin")]

    
        public class ManageExpertsController : Controller
    {
        DB_AlmaCmsEntities db;
        AspUserRepository repUSer;
        ExpertInfoRepository repExpertInfol;
        public ManageExpertsController()
        {
            db = new DB_AlmaCmsEntities();
            repUSer = new AspUserRepository(db);
            repExpertInfol = new ExpertInfoRepository(db);


        }
        public ManageExpertsController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        // GET: AdminManage/ManageExperts
        public ActionResult Index(string searchString, int? page)
        {
            var currentRoleID = db.AspNetRoles.Where(c => c.Name == "Expert").FirstOrDefault().Id;
            var ExpertList = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Expert")).ToList();
            var idList = ExpertList.Select(c => c.Id).ToList();

            var ExpertInfoList = repUSer.getExpertInfo().Where(c => idList.Contains(c.Id));
            if (!String.IsNullOrEmpty(searchString))
            {
                ExpertInfoList = ExpertInfoList.Where(c => c.Email.Contains(searchString)  || c.Name.Contains(searchString) || c.Family.Contains(searchString)).ToList();
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.searchString = searchString;
            return View(ExpertInfoList.ToPagedList(pageNumber, pageSize));
        }

        #region Create

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModels.Expert.VMAddExpert vmExpert)
        {

            if (!ModelState.IsValid)
            {
                return View(vmExpert);
            }

            var user = new ApplicationUser { UserName = vmExpert.Email, Email = vmExpert.Email, PhoneNumber = vmExpert.Mobile, PhoneNumberConfirmed = true };
            var result = UserManager.Create(user, vmExpert.Password);
            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Expert");
                var newInfo = new Models.ExpertInfo()
                {
                    Address = vmExpert.Address,
                    BirthDate = vmExpert.BirthDate,
                    Descriptions = vmExpert.Descriptions,
                    Family = vmExpert.Family,
                    Image = vmExpert.Image,
                    Name = vmExpert.Name,
                    Tel = vmExpert.Tel,
                    isActive = true,
                    NationalCode = vmExpert.NationalCode,
                    UserId = user.Id,


                };
                repExpertInfol.Insert(newInfo);

            }
            else
            {
                AddErrors(result);
                return View(vmExpert);
            }
            return RedirectToAction("index");
        }

        #endregion


        #region Edit

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentExpert = repUSer.getExpertInfo().Where(c => c.InfioID == (int)id).FirstOrDefault();
            if (currentExpert == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            ViewModels.Expert.VMEditExpert vmCurrentExpert = new ViewModels.Expert.VMEditExpert()
            {
                Address = currentExpert.Address,
                BirthDate = currentExpert.BirthDate,
                Descriptions = currentExpert.Descriptions,
                Family = currentExpert.Family,
                Image = currentExpert.Image,
                id = currentExpert.InfioID,
                Mobile = currentExpert.PhoneNumber,
                Name = currentExpert.Name,
                NationalCode = currentExpert.NationalCode,
                Tel = currentExpert.Tel,
                UserID = currentExpert.Id


            };




            return View(vmCurrentExpert);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModels.Expert.VMEditExpert vmExpertInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(vmExpertInfo);

            }


            var currentinfo = repExpertInfol.FindById(vmExpertInfo.id);

            currentinfo.Address = vmExpertInfo.Address;
            currentinfo.BirthDate = vmExpertInfo.BirthDate;
            currentinfo.Descriptions = vmExpertInfo.Descriptions;
            currentinfo.Family = vmExpertInfo.Family;
            currentinfo.Image = vmExpertInfo.Image;
            currentinfo.Name = vmExpertInfo.Name;
            currentinfo.NationalCode = vmExpertInfo.NationalCode;
            currentinfo.Tel = vmExpertInfo.Tel;
            repExpertInfol.Update(currentinfo);
            var currentuser = UserManager.FindById(vmExpertInfo.UserID);
            UserManager.SetPhoneNumber(vmExpertInfo.UserID, vmExpertInfo.Mobile);
            return RedirectToAction("index");

        }
        #endregion

        #region ChangePass
            public ActionResult ChangePass(string UserID)
        {
            var currentuser = UserManager.FindById(UserID);
            ViewModels.VMChangePassword vmchangePass = new VMChangePassword()
            {
                UserID = UserID,
                 UserName=currentuser.Email
            };

            return View(vmchangePass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
            public ActionResult ChangePass(VMChangePassword vmChangepass)
            {
                if(!ModelState.IsValid)
                {
                    return View(vmChangepass);
                }
                
                var result =  UserManager.ChangePassword(vmChangepass.UserID, vmChangepass.OldPassword, vmChangepass.NewPassword);
                if (result.Succeeded)
                {
                    
                    return RedirectToAction("Index");
                }
                AddErrors(result);
                return View(vmChangepass);

             
            }
            
        #endregion

        #region Delete
        public ActionResult Delete(string UserID)
        {
            if (UserID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repUSer.getExpertInfo().Where(c=>c.Id==UserID).FirstOrDefault();
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);



            return View(currentPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ViewModels.Expert.VMExpertInfo vmdeleteinfo)
        {
            var currentPage = repUSer.getExpertInfo().Where(c => c.Id == vmdeleteinfo.Id).FirstOrDefault();
            var currentuser = UserManager.FindById(vmdeleteinfo.Id);
            UserManager.Delete(currentuser);
            return RedirectToAction("Index");
        }
        #endregion
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}