using AlmaCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

using AlmaCMS.Helpers;
using AlmaCMS.Repositories;
using AlmaCMS.ViewModels;
using System.IO;
using System.Net;

namespace AlmaCMS.Controllers
{
    [Authorize(Roles = "Member")]
    public class UserProfileController : Controller
    {
        DB_AlmaCmsEntities db;
        UserInfoRepositiry repUserInfo;
        OrdersRepository repOrders;
        PaymentRepository repPayment;
        FavorisRepository repFavoriet;
        ShortMessageRepository repShorMessage;
        DiscountCodeRepository repDiscount;
        
        USerBagRepository repBag;
        public UserProfileController()
        {
            db = new DB_AlmaCmsEntities();
            repUserInfo = new UserInfoRepositiry(db);
            repOrders = new OrdersRepository(db);
            repPayment = new PaymentRepository(db);
            repBag = new USerBagRepository(db);
            repFavoriet = new FavorisRepository(db);
            repShorMessage = new ShortMessageRepository(db);
            repDiscount = new DiscountCodeRepository(db);
        }
        public UserProfileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        // GET: Profile
        public ActionResult Index()
        {
            if (UserManager.IsEmailConfirmed(User.Identity.GetUserId()))
            {
                ViewBag.EmailConfirmStatus = "True";
            }
            else
            {
                ViewBag.EmailConfirmStatus = "False";
            }

            if (UserManager.IsPhoneNumberConfirmed(User.Identity.GetUserId()))
            {
                ViewBag.PhoneConfirmStatus = "True";
            }
            else
            {
                ViewBag.PhoneConfirmStatus = "False";
            }


            string userID = User.Identity.GetUserId();

            var currentMemberInfo = new VMUserInfo();

            currentMemberInfo = repUserInfo.GetByUserID(userID).toVMUserInfo();

            ViewBag.currentUserInfo = currentMemberInfo;


            ViewBag.OrderCount = repOrders.getbymemberid(userID).Count;
            ViewBag.SuccesPaymentCount = repPayment.getbymemberid(userID).Count;
            return View(currentMemberInfo);
        }



        public ActionResult EditInfo()
        {




            ViewModels.VMUserInfo vminfo = new ViewModels.VMUserInfo();



            var currentInfo = repUserInfo.GetByUserID(User.Identity.GetUserId());
            if (currentInfo != null)
            {

                vminfo.Address = currentInfo.Address;
                vminfo.BirthDate = (DateTime)currentInfo.BirthDate;
                vminfo.Name = currentInfo.Name;
                vminfo.Phone = currentInfo.Phone;
                vminfo.UserId = currentInfo.UserId;
                vminfo.Mobile = currentInfo.Mobile;
                vminfo.CodeMelli = currentInfo.CodeMelli;





            }
            else
            {
                vminfo.Address = "";
                vminfo.BirthDate = new DateTime(1900,01,01);
                vminfo.Name = "";
                vminfo.CodeMelli = "";
                vminfo.Phone = "";
                vminfo.UserId = "";

            }

            return View(vminfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfo(ViewModels.VMUserInfo vminfo)
        {
            if (!ModelState.IsValid)
            {
                return View(vminfo);
            }

            var currentInfo = repUserInfo.GetByUserID(User.Identity.GetUserId());
            if (currentInfo != null)
            {

                currentInfo.Address = vminfo.Address;
                currentInfo.BirthDate = vminfo.BirthDate;
                currentInfo.Name = vminfo.Name;
                currentInfo.CodeMelli = vminfo.CodeMelli;
                currentInfo.Mobile = vminfo.Mobile;
                currentInfo.Phone = vminfo.Phone;


                repUserInfo.Update(currentInfo);

            }
            else
            {
                var newInfo = new Models.UserInfo();
                newInfo.Address = vminfo.Address;
                newInfo.BirthDate = vminfo.BirthDate;
                newInfo.Name = vminfo.Name;
                newInfo.CodeMelli = vminfo.CodeMelli;
                newInfo.Mobile = vminfo.Mobile;
                newInfo.Phone = vminfo.Phone;
                newInfo.UserId = User.Identity.GetUserId();

                repUserInfo.Insert(newInfo);


            }





            return RedirectToAction("index");
        }


        public ActionResult Orders()
        {

            var MemberList = repUserInfo.GetByUserID(User.Identity.GetUserId());
            var OrdersList = repOrders.GetDapperByMemberID(MemberList.UserId);

            return View(OrdersList);
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        public ActionResult Payments()
        {
            return View(repPayment.getbymemberid(User.Identity.GetUserId()));
        }

        public ActionResult Favorit()
        {
            var MemberId = User.Identity.GetUserId();

            //var FavoritProduct = repFavoriet.GetByMemberId(MemberId);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFavorit(int Favoritid)
        {
            var MemberId = User.Identity.GetUserId();
            var Favproduct = repFavoriet.FindById(Favoritid);

            repFavoriet.Delete(Favoritid);
            return RedirectToAction("Favorit");
        }


        #region UserBag


        public ActionResult ChargeReport()
        {

            var Userid = User.Identity.GetUserId();
            var Userbag = repBag.Where(c => c.userId == Userid).FirstOrDefault();


            var currentMemberInfo = new VMUserInfo();

            currentMemberInfo = repUserInfo.GetByUserID(Userid).toVMUserInfo();

            ViewBag.currentUserInfo = currentMemberInfo;
            ViewBag.BagInfo = Userbag;

            var BagHistory = db.BagTransactions.Where(c => c.BagId == Userbag.id).Where(c=>c.AddPrice>0).ToList();
            return View(BagHistory);
        }

        public ActionResult takeReport()
        {

            var Userid = User.Identity.GetUserId();
            var Userbag = repBag.Where(c => c.userId == Userid).FirstOrDefault();


            var currentMemberInfo = new VMUserInfo();

            currentMemberInfo = repUserInfo.GetByUserID(Userid).toVMUserInfo();

            ViewBag.currentUserInfo = currentMemberInfo;
            ViewBag.BagInfo = Userbag;

            var BagHistory = db.BagTransactions.Where(c => c.BagId == Userbag.id).Where(c=>c.MinusPrice>0).ToList();
            return View(BagHistory);
        }
        public  ActionResult UserBag()
        {

            var Userid=User.Identity.GetUserId();
            var Userbag = repBag.Where(c => c.userId == Userid).FirstOrDefault();
          

            var currentMemberInfo = new VMUserInfo();

            currentMemberInfo = repUserInfo.GetByUserID(Userid).toVMUserInfo();

            ViewBag.currentUserInfo = currentMemberInfo;
            ViewBag.BagInfo = Userbag;

            return View();
        }

        public ActionResult ChargeBag()
        {
            ViewBag.Success = 0;
            return View();
        }

        [HttpPost]
        public ActionResult ChargeBag(VMChargeBag vmcharge)
        {
            ViewBag.Success = 0;
            if(!ModelState.IsValid)
            {
                return View(vmcharge);
            }

            var CurrenrCode = repDiscount.Where(c => c.Code == vmcharge.DiscountCode).FirstOrDefault();
            if(CurrenrCode==null)
            {
                ModelState.AddModelError("Code", "کد وارد شذه صحیح نمی باشد");
                return View(vmcharge);
            }

            if(CurrenrCode.Used==true)
            {
                ModelState.AddModelError("Code", "کد وارد شذه قبلا استفاده شده است");
                return View(vmcharge);
            }

            var CurrentBag=repBag.Where(c=>c.userId==User.Identity.GetUserId()).FirstOrDefault();
            CurrentBag.BagPrice=CurrentBag.BagPrice+CurrenrCode.Discount_price;
            repBag.Update(CurrentBag);
            var newcharge = new Models.BagTransaction() { 
             AddPrice=CurrentBag.BagPrice,
              BagId=CurrentBag.id,
               dateInsert=DateTime.Now,
                Descriptions="کد تخفبف  "+ vmcharge.DiscountCode + " به مبلغ "+ CurrenrCode.Discount_price,
                 MinusPrice=0,
                  
            };
            db.BagTransactions.Add(newcharge);
            db.SaveChanges();
            CurrenrCode.Used = true;
            repDiscount.Update(CurrenrCode);
            ViewBag.Success = CurrenrCode.Discount_price;
            ModelState.Clear();
            return View(new VMChargeBag());
        }

        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion


        public async Task<ActionResult> Messages()
        {
            string UserId = User.Identity.GetUserId();
            VMShortMessage vmmessage = new VMShortMessage();
            var MEssageList = repShorMessage.GetByUserId(UserId).OrderByDescending(x => x.id).ToList();
            ViewBag.MEssageList = MEssageList;
            var currentuserinfo = repUserInfo.getDapperUserInfo().Where(c => c.Id == UserId).FirstOrDefault();

            ViewBag.UserInfo = currentuserinfo;
            return View(vmmessage);
        }

        [HttpPost]
        public async Task<ActionResult> Messages(VMShortMessage vmmessage)
        {
            string UserId = User.Identity.GetUserId();
            var MEssageList = repShorMessage.GetByUserId(UserId).OrderByDescending(x => x.id).ToList();
            ViewBag.MEssageList = MEssageList;
            var currentuserinfo = repUserInfo.getDapperUserInfo().Where(c => c.Id == UserId).FirstOrDefault();

            ViewBag.UserInfo = currentuserinfo;

            if (!ModelState.IsValid)
            {
                return View(vmmessage);
            }
            var newmessage = new Models.UsersShortMessage()
            {

                dateinsert = DateTime.Now,
                MessageBody = "",
                UserId = UserId,
                usermessage = vmmessage.Messagetext,

            };
            repShorMessage.Insert(newmessage);
            return RedirectToAction("Messages", new { UserId = vmmessage.USerId });

            return View(vmmessage);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}