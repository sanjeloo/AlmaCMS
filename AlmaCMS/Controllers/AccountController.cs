using AlmaCMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Repositories;
using AlmaCMS.ViewModels;
using System.Collections.Generic;
using System.IO;
using System;
using System.Data.Entity;
using AlmaCMS.Sms;

namespace AlmaCMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        DB_AlmaCmsEntities db;
        IntroductionRepository repIntroduction;
        StateRepository repState;
        UserInfoRepositiry repUserinfo;
        USerBagRepository repBah;
        public AccountController()
        {
            db=new DB_AlmaCmsEntities();
            repIntroduction = new IntroductionRepository(db);
            repState = new StateRepository(db);
            repUserinfo = new UserInfoRepositiry(db);

            repBah = new USerBagRepository(db);
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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


        public void SendSMS(string ToNumber, string strMEssage)
        {
            Helpers.SMSHelper.SendSMS(ToNumber, "ثبت نام", strMEssage);

        }
        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        AspNetUser a = db.AspNetUsers.FirstOrDefault();
                        ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);

                        HttpCookie shoppingcartCookie = new HttpCookie("name");
                        shoppingcartCookie.Value = repUserinfo.GetByUserID(user.Id).Name;
                        Response.SetCookie(shoppingcartCookie);
                        //user.FullName = repUserinfo.GetByUserID(user.Id).Name;
                        //if (UserManager.IsInRole(user.Id, "Suspend"))
                        //{
                        //    return RedirectToAction("index", "Home");
                        //}

                        //if (UserManager.IsInRole(user.Id, "Admin"))
                        //{
                        //    return RedirectToAction("Dashboard", "Admin", new { area = "manage" });
                        //}
                        //if (UserManager.IsInRole(user.Id, "Expert"))
                        //{
                        //    return RedirectToAction("Index", "profile", new { area = "Expert" });

                        //}
                        //if (UserManager.IsInRole(user.Id, "member"))
                        //{
                        //    return RedirectToAction("Index", "userprofile");

                        //}
                        return RedirectToLocal(returnUrl);

                    }

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [Route("Phonelogin")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Phonelogin(string phoneNumber)
        {
            try
            {
                var user = await db.AspNetUsers.FirstOrDefaultAsync(u => u.PhoneNumber==phoneNumber);


                if (phoneNumber.Length!=11 || user ==null)
                    return Json(new { Success = false, Message = "شماره تلفن یافت نشد" });

                var t = await db.TempUserLogins.FirstOrDefaultAsync(tu => tu.PhoneNumber==phoneNumber);
                if (t!=null)
                    db.TempUserLogins.Remove(t);
                int code = new Random().Next(10000, 99999);
                db.TempUserLogins.Add(new TempUserLogin
                {
                    ExpirationDate = DateTime.Now.AddMinutes(3),
                    PhoneNumber = phoneNumber,
                    UserId= user.Id,
                    Code = code
                }) ;
                await db.SaveChangesAsync();
                SendSms.SendVerification(phoneNumber, code.ToString());
                return Json(new { Success = true, Message = "کد تایید ارسال شد" });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Message = "خطایی رخ داده" });
            }
        }

        [Route("VerifyPhonelogin")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyPhonelogin(string phoneNumber, string code, string url)
        {
            try
            {

                if (phoneNumber.Length!=11 || code.Length!=5)
                    return Json(new { Success = false, Message = "کد نامعتبر" });


                int comparableCode = int.Parse(code);
                var tempUser = await db.TempUserLogins.FirstOrDefaultAsync(tu => tu.PhoneNumber==phoneNumber
                && tu.ExpirationDate>DateTime.Now);
                if (tempUser==null)
                    return Json(new { Success = false, Message = "کد منقضی شده است" });
                if (tempUser.Code !=comparableCode)
                    return Json(new { Success = false, Message = "کد وارد شده اشتباه است" });

                ApplicationUser user = await UserManager.FindByIdAsync(tempUser.UserId);
                db.TempUserLogins.Remove(tempUser);
                await db.SaveChangesAsync();
                await SignInManager.SignInAsync(user, true, true);

                HttpCookie shoppingcartCookie = new HttpCookie("name");
                shoppingcartCookie.Value = repUserinfo.GetByUserID(user.Id).Name;
                Response.SetCookie(shoppingcartCookie);
                // user.FullName = repUserinfo.GetByUserID(user.Id).Name;
                string Url = String.IsNullOrEmpty(url) ? "/Home/Index" : url;
                //if (UserManager.IsInRole(tempUser.UserId, "Suspend"))
                //{
                //    Url = "/Index/Home";
                //}

                if (UserManager.IsInRole(tempUser.UserId, "Admin"))
                {
                    Url = "/Manage/Admin/Dashboard";
                }
                //if (UserManager.IsInRole(tempUser.UserId, "Expert"))
                //{
                //    Url = "/Expert/profile/Index";

                //}
                //if (UserManager.IsInRole(tempUser.UserId, "Member"))
                //{
                //    Url="/userprofile/Index";
                //}

                return Json(new { Success = true, Message = "", Url = Url });

            }
            catch (Exception e)
            {

                throw;
            }
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [AllowAnonymous]
        public ActionResult UserResetPassword()
        {
            ViewBag.message = "0";
            return View();

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UserResetPassword(VMResetPassword vmreset)
        {

            if (!ModelState.IsValid)
            {
                return View(vmreset);
            }

            var Currentuser = db.AspNetUsers.Where(c => c.UserName == vmreset.MobileNumber).ToList();
            if (Currentuser.Count==0)
            {
                ModelState.AddModelError("MobileNumber", "شماره موبایل وارد شده موجود نمی باشد");
                return View(vmreset);
            }
            var code = UserManager.GeneratePasswordResetToken(Currentuser.FirstOrDefault().Id);
            var NewPass = Helpers.CreateHash.RNGCharacterMaskPass();
            var result = UserManager.ResetPassword(Currentuser.FirstOrDefault().Id, code, NewPass);
            ViewBag.message = "1";
            SendSMS(vmreset.MobileNumber, "رمز عبور جدید شما در سایت : " + NewPass);
            ModelState.Clear();

            return View(new VMResetPassword());

        }
        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(VMRegisterMember model)
        {
            try
            {


                if (string.IsNullOrEmpty(model.Email))
                {
                    model.Email = model.MobileNumber+"@dartkala.com";
                }
                if (ModelState.IsValid)
                {
                    if (await db.AspNetUsers.AnyAsync(u => u.PhoneNumber == model.MobileNumber))
                    {
                        ModelState.AddModelError("MobileNumber", "نام کاربری تکراری می باشد");
                        return View(model);
                    }
                    var result = await UserManager.CreateAsync(new ApplicationUser
                    {
                        UserName = model.MobileNumber,
                        Email = model.Email,
                        PhoneNumber=model.MobileNumber
                    });
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("MobileNumber", "خطایی رخ داده");
                        return View(model);
                    }
                    var user = await UserManager.FindByNameAsync(model.MobileNumber);
                    await UserManager.AddToRoleAsync(user.Id, "Member");
                    var newInfo = new UserInfo()
                    {
                        UserId=user.Id,
                        Mobile=model.MobileNumber,
                        Name=model.Name,
                        Tel=model.MobileNumber,
                        Address = "",
                        BirthDate = DateTime.Now.AddYears(-30),
                        City = "",
                        CodeMelli = "",
                        Email  = user.Email,
                        IntroductionTypeId = 1,
                        Phone = model.MobileNumber,
                        PostalCode = "",
                        ReagentCode = 0,
                        SatetId = 0

                    };

                    repUserinfo.Insert(newInfo);
                    repBah.Insert(new UserBag()
                    {
                        BagPrice=0,
                        userId = user.Id
                    });

                    return View("login");

                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                ViewBag.Link = callbackUrl;
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}