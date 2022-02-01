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
    [HasPermission("ManageAdmins")]

    public class ManageAdminsController : Controller
    {
        // GET: AdminManage/ManageAdmins

        DB_AlmaCmsEntities db;
        AspUserRepository repUSer;
        ExpertInfoRepository repExpertInfol;

        UserInfoRepositiry repUSerInfo;
        AdminRolesRepository repAdminRoles;
        RolesRepository repRoles;

        public ManageAdminsController()
        {
            db = new DB_AlmaCmsEntities();
            repUSer = new AspUserRepository(db);
            repExpertInfol = new ExpertInfoRepository(db);
            repUSerInfo = new UserInfoRepositiry(db);

            repRoles = new RolesRepository(db);
            repAdminRoles = new AdminRolesRepository(db);
        }

        public ManageAdminsController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        public ActionResult Index(string searchString, int? page)
        {
            var currentRoleID = db.AspNetRoles.Where(c => c.Name == "Admin").FirstOrDefault().Id;
            var AdminList = db.AspNetUsers.Where(x => x.AspNetRoles.Select(y => y.Name).Contains("Admin")).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                AdminList = AdminList.Where(c => c.Email.Contains(searchString)).ToList();
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.searchString = searchString;
            return View(AdminList.ToList());
        }

        #region Create

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel vnAdmin)
        {

            if (!ModelState.IsValid)
            {
                return View(vnAdmin);
            }

            var user = new ApplicationUser { UserName = vnAdmin.Mobile, Email = vnAdmin.Mobile+ "@pusnazari.com",PhoneNumber=vnAdmin.Mobile  };
            var result = UserManager.Create(user, vnAdmin.Password);
            if (result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Admin");

                repUSerInfo.Insert(new UserInfo() { 
                
                 Address="",
                  BirthDate=new DateTime(1900,01,01),
                   City="",
                    CodeMelli="",
                     Email="",
                      IntroductionTypeId=1,
                       Mobile=vnAdmin.Mobile,
                        Name=vnAdmin.Name,
                         Phone=vnAdmin.Mobile,
                          PostalCode="",
                           SatetId=1,
                            Tel="",
                             UserId=user.Id,
                            
                                              
                });


            }
            else
            {
                AddErrors(result);
                return View(vnAdmin);
            }
            return RedirectToAction("index");
        }


        #region Delete
        public ActionResult Delete(string UserID)
        {
            if (UserID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var currentPage = repUSer.Where(c => c.Id == UserID).FirstOrDefault();
            if (currentPage == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);



            return View(currentPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AlmaCMS.Models.AspNetUser vmdeleteinfo)
        {

            var currentuser = UserManager.FindById(vmdeleteinfo.Id);
            UserManager.Delete(currentuser);
            return RedirectToAction("Index");
        }
        #endregion

        #region ChangePass
        public ActionResult ChangePass(string UserID)
        {
            var currentuser = UserManager.FindById(UserID);
            ViewModels.VMChangePassword vmchangePass = new VMChangePassword()
            {
                UserID = UserID,
                UserName = currentuser.UserName
            };

            return View(vmchangePass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(VMChangePassword vmChangepass)
        {
            if (!ModelState.IsValid)
            {
                return View(vmChangepass);
            }

            var result = UserManager.ChangePassword(vmChangepass.UserID, vmChangepass.OldPassword, vmChangepass.NewPassword);
            if (result.Succeeded)
            {

                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(vmChangepass);


        }

        #endregion


        #region ChangeAccess
        public ActionResult ChangeAccess(string UserID)
        {


            var Currentuser = repUSer.Where(c=>c.Id==UserID).FirstOrDefault();

            ViewModels.VMChangeUserAccess vmChangeAccess = new VMChangeUserAccess()
            {
                UserID = UserID,
                UserName = Currentuser.UserName
            };

            var featuresItems = db.Roles.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Comment,
            });
            ViewBag.featuresItems = featuresItems;
            var RoleIn = db.AdminRoles.Where(c => c.UserId == UserID).ToList();
            ViewBag.FeaturesIn = RoleIn;
            string strRole = "";
            foreach (var fitem in RoleIn)
            {
                strRole += fitem.RoleId + ",";
            }
            vmChangeAccess.SelectedAccess = strRole;

            var featuresItemsList = db.Roles.Select(a => new SelectListItem
            {

                Value = a.Id.ToString(),
                Text = a.Comment
            });
            if (!string.IsNullOrEmpty(vmChangeAccess.SelectedAccess))
            {
                var ArrRoles = vmChangeAccess.SelectedAccess.Split(',');
                List<SelectListItem> NewLoist = new List<SelectListItem>();
                foreach (var itemrole in featuresItemsList)
                {
                    if (ArrRoles.Where(c => c.Equals(itemrole.Value)).Count() > 0)
                    {
                        itemrole.Selected = true;

                    }
                    NewLoist.Add(itemrole);
                }


                ViewBag.featuresItems = NewLoist;
            }
            else
            {
                ViewBag.featuresItems = featuresItemsList;
            }


            return View(vmChangeAccess);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAccess(VMChangeUserAccess vmChaneAccess)
        {
            var featuresItemsList = db.Roles.Select(a => new SelectListItem
            {

                Value = a.Id.ToString(),
                Text = a.Comment
            });
            if (!string.IsNullOrEmpty(vmChaneAccess.SelectedAccess))
            {
                var ArrRoles = vmChaneAccess.SelectedAccess.Split(',');
                List<SelectListItem> NewLoist = new List<SelectListItem>();
                foreach (var itemrole in featuresItemsList)
                {
                    if (ArrRoles.Where(c => c.Equals(itemrole.Value)).Count() > 0)
                    {
                        itemrole.Selected = true;

                    }
                    NewLoist.Add(itemrole);
                }


                ViewBag.featuresItems = NewLoist;
            }
            else
            {
                ViewBag.featuresItems = featuresItemsList;
            }
            if (!ModelState.IsValid)
            {
                return View(vmChaneAccess);
            }


            InsertAccess(vmChaneAccess.UserID, vmChaneAccess.SelectedAccess);
            return RedirectToAction("Index");






        }

        private void InsertAccess(string UserID, string AccessList)
        {

            db.AdminRoles.RemoveRange(db.AdminRoles.Where(fs => fs.UserId == UserID));
            db.SaveChanges();
            if (AccessList != "" && AccessList != null)
            {
                var ArrFeatures = AccessList.Split(',');
                if (ArrFeatures[ArrFeatures.Length - 1] == "" || ArrFeatures[ArrFeatures.Length - 1] == null)
                {
                    ArrFeatures = ArrFeatures.Where(a => a != "" && a != null).ToArray();
                }
                foreach (var item in ArrFeatures)
                {
                    if (item != "")
                    {
                        AdminRole Newfeature = new AdminRole();
                        //NewRole.Id = int.Parse(item);
                        Newfeature.UserId = UserID;

                        //var roleId = db.Roles.FirstOrDefault(r => r.Id == int.Parse(item));
                        Newfeature.RoleId = int.Parse(item);
                        db.AdminRoles.Add(Newfeature);
                        db.SaveChanges();
                    }
                }
            }

        }
        #endregion
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

    }


   
}
