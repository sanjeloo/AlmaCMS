using AlmaCMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlmaCMS;
using AlmaCMS.Repositories;
using System.IO;
using AlmaCMS.Helpers;
using AlmaCMS.ViewModels;

namespace AlmaCMS.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]

    [HasPermission("ManageUsers")]
    public class ManageUsersController : Controller
    {
        DB_AlmaCmsEntities db;
        UserInfoRepositiry repUserInfo;
        ShortMessageRepository repShorMessage;
        public ManageUsersController()
        {
            db = new DB_AlmaCmsEntities();
            repUserInfo = new UserInfoRepositiry(db);
            repShorMessage = new ShortMessageRepository(db);

        }
        public ManageUsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        // GET: Admin/ManageUsers
        public async Task<ActionResult> Index(string Title, string Filter)
        {


            ViewBag.USerInfoList = repUserInfo.getDapperUserInfo().ToList();
            List<ViewModels.VMDropDownOption> vmRolesList = new List<ViewModels.VMDropDownOption>();

            ViewBag.RolesListAl = db.AspNetRoles.ToList();
            foreach (var item in RoleManager.Roles.ToList())
            {
                vmRolesList.Add(new ViewModels.VMDropDownOption()
                {

                    Title = item.Name
                });
            }
            ViewBag.RoleList = vmRolesList;

            if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Filter))
            {
                ViewBag.StatusTitle = "همه کاربران";
                return View(await UserManager.Users.ToListAsync());
            }

            if (!string.IsNullOrEmpty(Filter))
            {
                ViewBag.StatusTitle = "کاربر  : " + Filter;
                var users = UserManager.Users
.Where(x => x.Email.Contains(Filter) || x.UserName.Contains(Filter)).ToList();
                return View(users);

            }

            var Currentrole = RoleManager.FindByName(Title);
            if (Currentrole != null)
            {
                ViewBag.StatusTitle = Title;
                string RoleID = Currentrole.Id;
                var users = UserManager.Users
.Where(x => x.Roles.Select(y => y.RoleId).Contains(RoleID))
.ToList();



                return View(users);
            }
            else
            {
                ViewBag.StatusTitle = "همه کاربران";
                return View(await UserManager.Users.ToListAsync());
            }



        }


        public async Task<ActionResult> Messages(string UserId)
        {

            VMShortMessage vmmessage = new VMShortMessage();
            var MEssageList = repShorMessage.GetByUserId(UserId).OrderByDescending(x=>x.id).ToList();
            ViewBag.MEssageList = MEssageList;
            var currentuserinfo=repUserInfo.getDapperUserInfo().Where(c=>c.Id==UserId).FirstOrDefault();

            ViewBag.UserInfo = currentuserinfo;
            return View(vmmessage);
        }

        [HttpPost]
        public async Task<ActionResult> Messages(VMShortMessage vmmessage)
        {
            var MEssageList = repShorMessage.GetByUserId(vmmessage.USerId).OrderByDescending(x => x.id).ToList();
            ViewBag.MEssageList = MEssageList;
            var currentuserinfo = repUserInfo.getDapperUserInfo().Where(c => c.Id == vmmessage.USerId).FirstOrDefault();

            ViewBag.UserInfo = currentuserinfo;

            if(!ModelState.IsValid)
            {
                return View(vmmessage);
            }
            var newmessage = new Models.UsersShortMessage() { 
            
             dateinsert=DateTime.Now,
              MessageBody=vmmessage.Messagetext,
               UserId=vmmessage.USerId,
                usermessage="",
               
            };
            repShorMessage.Insert(newmessage);
            return RedirectToAction("Messages", new { UserId = vmmessage.USerId });
        
            return View(vmmessage);
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            var UserInfo = repUserInfo.GetByUserID(user.Id);

            if (UserInfo != null)
            {
                ViewBag.UserInfo = UserInfo;
            }
            else
            {
                ViewBag.UserInfoStatus = "False";
            }

            if (user.EmailConfirmed)
            {
                ViewBag.EmailStatus = "1";
            }
            else
            {
                ViewBag.EmailStatus = "0";
            }
            if (user.PhoneNumberConfirmed)
            {
                ViewBag.PhoneStatus = "1";
            }
            else
            {
                ViewBag.PhoneStatus = "0";
            }

            ViewBag.MobileNumber = user.PhoneNumber;
            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };


                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                if (selectedRole.FirstOrDefault() == "Confirmed")
                {
                    //Helpers.SMSHelper.SendFastSMS(user.PhoneNumber, "", "2059");
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }


        private void SendConfirmRmail(string toEmail)
        {

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            string CurrentDate = pc.GetYear(DateTime.Now) + "/" + pc.GetMonth(DateTime.Now) + "/" + pc.GetDayOfMonth(DateTime.Now);

            StreamReader sr = new StreamReader(HttpContext.Server.MapPath("/Content/emailtemp/email-format.html"));
            string SampleMail = sr.ReadToEnd();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("سلام.");
            sb.Append("</br>");
            sb.Append("کاربر محترم، ورود شما را به سامانه جامع خرید و فروش ارزدیجیتال 24change.ir خوش آمد می‌گوییم. امیدواریم بتوانیم تجربه بی‌نظیری از معاملات ارزهای دیجیتال را برای شما فراهم کنیم. ");
            sb.Append("</br>");
            sb.Append("برای آشنایی با قوانین سایت ازلینک زیر استفاده نمایید: ");
            sb.Append("</br>");
            sb.Append("<a href=\"" + "http://24change.ir/Pages/2/%D9%82%D9%88%D8%A7%D9%86%DB%8C%D9%86-%D8%B3%D8%A7%DB%8C%D8%AA" + "\">قوانین سایت </a>");
            sb.Append("</br>");
            sb.Append("برای آشنایی با نحوه خرید و فروش در سایت از این لینک استفاده کنید: ");
            sb.Append("</br>");
            sb.Append("<a href=\"" + "http://24change.ir/Pages/3/%D9%82%D9%88%D8%A7%D9%86%DB%8C%D9%86-%D9%86%D9%82%D9%84-%D9%88%D8%A7%D9%86%D8%AA%D9%82%D8%A7%D9%84%D8%A7%D8%AA-%D8%A8%D8%A7%D9%86%DA%A9%DB%8C-%D8%B1%DB%8C%D8%A7%D9%84%DB%8C" + "\">نحوه واریز و برداشت </a>");
            sb.Append("</br>");
            sb.Append("برای اطلاعات بیشتر به " + " <a href=\"" + "https://t.me/supp_24change" + "\">کانال ما در تلگرام  </a> " + " در تلگرام بپیوندید.  ");
            sb.Append("</br>");
            sb.Append("سپاس از همراهی شما،");
            sb.Append("</br>");
            sb.Append("تیم  24change.ir: ");
            sb.Append("</br>");
            sb.Append("<a href=\"" + "http://24change.ir/" + "\">www.24change.ir </a>");
            sb.Append("</br>");

            sb.Append("تماس با ما :");
            sb.Append("</br>");
            sb.Append("<a href=\"" + "mailto:support@24change.ir" + "\">support@24change.ir </a>");
            sb.Append("</br>");
            sb.Append("پشتیبانی در تلگرام: :");
            sb.Append("</br>");
            sb.Append("<a href=\"" + "https://t.me/info24change" + "\">t.me/info24change </a>");


            SampleMail = SampleMail.Replace("{{EmailTitle}}", "به 24change.ir خوش آمدید.");
            SampleMail = SampleMail.Replace("{{EmailBody}}", sb.ToString());
            SampleMail = SampleMail.Replace("{{EmailDate}}", CurrentDate.ToString());
            Helpers.CustomEmail.sendemail("به 24change.ir خوش آمدید", toEmail, SampleMail);
        }
        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }




        public ActionResult EditInfo(string id)
        {




            ViewModels.VMUserInfo vminfo = new ViewModels.VMUserInfo();

            var currentuser = UserManager.FindById(id);

            ViewBag.currentEmail = currentuser.Email;

            var currentInfo = repUserInfo.GetByUserID(id);
            if (currentInfo != null)
            {

                //vminfo.BankCartNo = currentInfo.BankCartNo;
                //vminfo.BankCartPhoto = currentInfo.BankKartPhoto;
                //vminfo.Name = currentInfo.Name;
                //vminfo.NationalCartImage = currentInfo.NationalCartImage;
                //vminfo.NationalCode = currentInfo.NationalCode;
                //vminfo.NickName = currentInfo.NickName;
                //vminfo.ShabaNumbe = currentInfo.ShabaNumbe;
                //vminfo.StaticPhone = currentInfo.StaticPhone;
                //vminfo.UserID = currentInfo.UserId;
                //vminfo.WalletID = "";

                //vminfo.BankName = currentInfo.BankName;
                //vminfo.id = currentInfo.id;

            }
            else
            {

                //vminfo.BankCartNo = "";
                //vminfo.BankCartPhoto = "";
                //vminfo.Name = "";
                //vminfo.NationalCartImage = "";
                //vminfo.NationalCode = "";
                //vminfo.NickName = "";
                //vminfo.ShabaNumbe = "";

                //vminfo.WalletID = "";
                //vminfo.BankName = "";
                //vminfo.UserID = currentuser.Id;
                //vminfo.id = 0;
                //vminfo.StaticPhone = "";
            }

            return View(vminfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfo(ViewModels.VMUserInfo vminfo)
        {
            var currentuser = UserManager.FindById(vminfo.UserId);

            ViewBag.currentEmail = currentuser.Email;
            vminfo.id = 0;
            ModelState.Remove("id");
            if (!ModelState.IsValid)
            {
                return View(vminfo);
            }


            var currentInfo = repUserInfo.GetByUserID(vminfo.UserId);
            if (currentInfo != null)
            {

                //currentInfo.BankCartNo = vminfo.BankCartNo;
                //currentInfo.BankKartPhoto = vminfo.BankCartPhoto;
                //currentInfo.Name = vminfo.Name;
                //currentInfo.NationalCartImage = vminfo.NationalCartImage;
                //currentInfo.NationalCode = vminfo.NationalCode;
                //currentInfo.NickName = vminfo.NickName;
                //currentInfo.ShabaNumbe = vminfo.ShabaNumbe;
                //currentInfo.UserId = vminfo.UserID;
                //currentInfo.WalletID = vminfo.WalletID;
                //currentInfo.StaticPhone = vminfo.StaticPhone;

                //currentInfo.BankName = vminfo.BankName;

                //repUserInfo.Update(currentInfo);

            }
            else
            {
                //var newInfo = new Models.UserInfo();
                //newInfo.BankCartNo = vminfo.BankCartNo;
                //newInfo.BankKartPhoto = vminfo.BankCartPhoto;
                //newInfo.Name = vminfo.Name;
                //newInfo.NationalCartImage = vminfo.NationalCartImage;
                //newInfo.NationalCode = vminfo.NationalCode;
                //newInfo.NickName = vminfo.NickName;
                //newInfo.ShabaNumbe = vminfo.ShabaNumbe;
                //newInfo.UserId = vminfo.UserID;
                //newInfo.WalletID = vminfo.WalletID;
                //newInfo.StaticPhone = vminfo.StaticPhone;

                //newInfo.BankName = vminfo.BankName;
                //repUserInfo.Insert(newInfo);


            }
            var currentuserID = vminfo.UserId;
            UserManager.RemoveFromRole(currentuserID, "NewUser");
            UserManager.RemoveFromRole(currentuserID, "Confirmed");
            UserManager.AddToRole(currentuserID, "Waiting");
            //Helpers.CustomEmail.sendemail("تکمیل اطلاعات کاربر :" + User.Identity.Name, System.Web.Configuration.WebConfigurationManager.AppSettings["ReceiveEmail"].ToString(), "تکمیل اطلاعات  <br> " + User.Identity.Name);

            return RedirectToAction("index");
        }

        public ActionResult DeleteMessage(int id)
        {
            var currentMessage = repShorMessage.FindById(id);
            var UserId = currentMessage.UserId;
            repShorMessage.Delete(id);
            return RedirectToAction("Messages", new { UserId = UserId });
          
        }

        public ActionResult SubUsers(string UserId)
        {


            var vmSubusers = new VMExpertSuUsers() { ExpertId=UserId };
            var CurrentUser = repUserInfo.getDapperUserInfo().Where(c => c.Id == UserId).FirstOrDefault();
            ViewBag.UserInfo = CurrentUser;
            var Currentrole = RoleManager.FindByName("Expert");
            string RoleID = Currentrole.Id;
            var vmUserList = new List<VMDropDownOption>();

            var Userist = repUserInfo.getDapperUserInfo().Where(c => c.RoleId == RoleID && c.Id!=UserId).ToList();
            var featuresItems = Userist.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            ViewBag.featuresItems = featuresItems;
            var USerIn = db.ExpertSubUsers.Where(c => c.ExpertId == UserId).ToList();
            ViewBag.FeaturesIn = USerIn;
            string strRole = "";
            foreach (var fitem in USerIn)
            {
                strRole += fitem.SubUserId + ",";
            }
            vmSubusers.SelectedUsers = strRole;

        
            if (!string.IsNullOrEmpty(vmSubusers.SelectedUsers))
            {
                var ArrRoles = vmSubusers.SelectedUsers.Split(',');
                List<SelectListItem> NewLoist = new List<SelectListItem>();
                foreach (var itemrole in featuresItems)
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
                ViewBag.featuresItems = featuresItems;
            }

            return View(vmSubusers);

        }
        [HttpPost]
        public ActionResult SubUsers(VMExpertSuUsers vmexpertusers)
        {


            
            var CurrentUser = repUserInfo.getDapperUserInfo().Where(c => c.Id == vmexpertusers.ExpertId).FirstOrDefault();
            ViewBag.UserInfo = CurrentUser;
            var Currentrole = RoleManager.FindByName("Expert");
            string RoleID = Currentrole.Id;
            var vmUserList = new List<VMDropDownOption>();

            var Userist = repUserInfo.getDapperUserInfo().Where(c => c.RoleId == RoleID && c.Id != vmexpertusers.ExpertId).ToList();
            var featuresItems = Userist.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            ViewBag.featuresItems = featuresItems;
            var USerIn = db.ExpertSubUsers.Where(c => c.ExpertId == vmexpertusers.ExpertId).ToList();
            ViewBag.FeaturesIn = USerIn;
            string strRole = "";
            foreach (var fitem in USerIn)
            {
                strRole += fitem.SubUserId + ",";
            }



            if (!string.IsNullOrEmpty(vmexpertusers.SelectedUsers))
            {
                var ArrRoles = vmexpertusers.SelectedUsers.Split(',');
                List<SelectListItem> NewLoist = new List<SelectListItem>();
                foreach (var itemrole in featuresItems)
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
                ViewBag.featuresItems = featuresItems;
            }

            if(ModelState.IsValid)
            {
                InsertSubUsers(vmexpertusers.ExpertId, vmexpertusers.SelectedUsers);
                return RedirectToAction("index");
            }
            else
            {
                return View(vmexpertusers);
            }
           

        }

        private void InsertSubUsers(string ExpertId, string UserList)
        {

            db.ExpertSubUsers.RemoveRange(db.ExpertSubUsers.Where(fs => fs.ExpertId == ExpertId));
            db.SaveChanges();
            if (UserList != "" && UserList != null)
            {
                var ArrFeatures = UserList.Split(',');
                if (ArrFeatures[ArrFeatures.Length - 1] == "" || ArrFeatures[ArrFeatures.Length - 1] == null)
                {
                    ArrFeatures = ArrFeatures.Where(a => a != "" && a != null).ToArray();
                }
                foreach (var item in ArrFeatures)
                {
                    if (item != "")
                    {
                        ExpertSubUser Newfeature = new ExpertSubUser();
                        //NewRole.Id = int.Parse(item);
                        Newfeature.ExpertId = ExpertId;
                        
                        //var roleId = db.Roles.FirstOrDefault(r => r.Id == int.Parse(item));
                        Newfeature.SubUserId = (item);
                        db.ExpertSubUsers.Add(Newfeature);
                        db.SaveChanges();
                    }
                }
            }

        }
    }
}