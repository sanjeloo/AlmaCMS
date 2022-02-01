using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace AlmaCMS.Controllers
{
    [Authorize(Roles = "Member,Expert")]
    public class ShippingController : Controller
    {
        DB_AlmaCmsEntities db;
        UserInfoRepositiry repMembers;
        SendTypeRepository repSendType;
        OrdersRepository repOrder;
        OrderProductRepository repOP;
        SiteSettingRepository repsitsetting;
        StateRepository repState;
        CityRepository repCity;
        PaymentRepository repPayment;


        public ShippingController()
        {
            db = new DB_AlmaCmsEntities();
            repSendType = new SendTypeRepository(db);
            repMembers = new UserInfoRepositiry(db);
            repOrder = new OrdersRepository(db);
            repOP = new OrderProductRepository(db);
            repsitsetting = new SiteSettingRepository(db);
            repState = new StateRepository(db);
            repPayment = new PaymentRepository(db);
            repCity = new CityRepository(db);

        }
        // GET: Shipping
        public ActionResult Index()
        {
            var generalsetting = repsitsetting.GetAll().FirstOrDefault();
            double TotalPrice = 0;
            double totalWeight = 0;
            List<VMCartItem> cartitems = new List<VMCartItem>();
            if (this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"] != null)
            {
                HttpCookie ShoppingCartCoolkie = this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"];

                var jsCartItems = Server.UrlDecode(ShoppingCartCoolkie.Value.ToString());
                cartitems = JsonConvert.DeserializeObject<List<VMCartItem>>(jsCartItems);
            }

            foreach (var Item in cartitems)
            {
                TotalPrice = TotalPrice + (long)(Item.Product.Price * Item.Quantity);
                //totalWeight = totalWeight + (long)(Item.Product.weight * Item.Quantity);
            }

            VMShipping newshipping = new VMShipping();
            var currentuserInfo = repMembers.GetByUserID(User.Identity.GetUserId());
            var currentuser = new VMUserInfo();
            if (currentuserInfo != null)
            {
                currentuser = currentuserInfo.toVMUserInfo();
            }
            newshipping.DeliverName = currentuser.Name;
            newshipping.DeliverMobile = currentuser.Mobile;
            newshipping.DeliverTel = currentuser.Phone;
            newshipping.Address = currentuser.Address;
            newshipping.MemberID = currentuser.UserId;
            newshipping.SendTypeID = 0;
            newshipping.PaymentTypeid = 1;
            newshipping.DateInsert = DateTime.Now;
            newshipping.StateID = 0;

            double VatPrice = 0;

            if (generalsetting.VAT == true)
            {
                VatPrice = (((double)TotalPrice * (double)generalsetting.VatPercent) / 100);

            }


            ViewBag.Vat = VatPrice;

            ViewBag.totalPrice = TotalPrice;
            ViewBag.TotalPriceWithVat = TotalPrice + VatPrice;
            var SendTypeList = repSendType.GetAll().ToList();

            double basePrice = (double)SendTypeList.FirstOrDefault().Price;
            //SendTypeList.Where(c => c.id == 6).FirstOrDefault().Price = (long)CalPostPrice(totalWeight, basePrice, generalsetting);

            ViewBag.SendypeList = SendTypeList;
            var statelist = repState.GetAll().OrderBy(c => c.Title).ToList();
            List<State> allstateList = new List<State>();
            allstateList.Add(new State() { id = 0, Title = "انتخاب استان", Comments = "انتخاب استان" });
            foreach (var item in statelist)
            {
                allstateList.Add(item);
            }
            ViewBag.StateList = allstateList;

            ViewBag.TotalWeight = totalWeight;
            newshipping.SendTypePrice = 0;




            List<VMCity> vmCityList = new List<VMCity>();

            vmCityList.Add(new VMCity() { Id = 0, Title = "انتخاب شهر" });

            ViewBag.cityList = vmCityList;

            return View(newshipping);
        }
        [HttpPost]
        public ActionResult Index(VMShipping vmshipping)
        {
            var generalsetting = repsitsetting.GetAll().FirstOrDefault();

            var SiteSetting = db.SiteSettings.FirstOrDefault();


            long TotalPrice = 0;
            double totalWeight = 0;
            var SendPrice = 0;
            if (vmshipping.SendTypeID > 0)
                SendPrice = (int)repSendType.FindById(vmshipping.SendTypeID).Price;
            var currentuserInfo = repMembers.GetByUserID(User.Identity.GetUserId());
            var currentuser = new VMUserInfo();
            if (currentuserInfo != null)
            {
                currentuser = currentuserInfo.toVMUserInfo();
            }


            List<VMCartItem> cartitems = new List<VMCartItem>();
            if (this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"] != null)
            {
                HttpCookie ShoppingCartCoolkie = this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"];

                var jsCartItems = Server.UrlDecode(ShoppingCartCoolkie.Value.ToString());
                cartitems = JsonConvert.DeserializeObject<List<VMCartItem>>(jsCartItems);
            }
            long Productsprice = 0;
            foreach (var Item in cartitems)
            {
                TotalPrice = TotalPrice + (long)(Item.Product.Price * Item.Quantity);
                //totalWeight = totalWeight + (long)(Item.Product.weight * Item.Quantity);
                totalWeight = 0;
            }

            Productsprice = TotalPrice;

            double VatPrice = 0;

            if (SiteSetting.VAT == true)
            {
                VatPrice = ((TotalPrice * (double)SiteSetting.VatPercent) / 100);

            }

            //if(vmshipping.SendTypeID==6)
            //{
            //    if(string.IsNullOrEmpty(vmshipping.PostalCode))
            //    {
            //        ModelState.AddModelError("PostalCode", "کد پستی را وارد نمایید");
            //    }
            //}


            List<VMCity> vmCityList = new List<VMCity>();

            vmCityList.Add(new VMCity() { Id = 0, Title = "انتخاب شهر" });


            if (vmshipping.StateID > 0)
            {
                var CityList = repCity.getByGroupID(vmshipping.StateID);
                foreach (var item in CityList)
                {
                    vmCityList.Add(item.toVMCity());

                }
            }
            ViewBag.cityList = vmCityList;
            if (!ModelState.IsValid)
            {



                ViewBag.totalPrice = TotalPrice;
                ViewBag.TotalPriceWithVat = TotalPrice + VatPrice;
                var SendTypeList = repSendType.GetAll().ToList();

                double basePrice = (double)SendTypeList.FirstOrDefault().Price;
                //SendTypeList.Where(c => c.id == 6).FirstOrDefault().Price = (long)CalPostPrice(totalWeight, basePrice, generalsetting);

                ViewBag.SendypeList = SendTypeList;
                var statelist = repState.GetAll().OrderBy(c => c.Title).ToList();
                List<State> allstateList = new List<State>();
                allstateList.Add(new State() { id = 0, Title = "انتخاب استان", Comments = "انتخاب استان" });
                foreach (var item in statelist)
                {
                    allstateList.Add(item);
                }
                ViewBag.StateList = allstateList;
                ViewBag.TotalWeight = totalWeight;
                if (!ModelState.IsValidField("StateID"))
                {
                    ModelState.Remove("StateID");
                    ModelState.AddModelError("StateID", "استان را انتخاب نمایید");
                }

                if (!ModelState.IsValidField("SendTypeID"))
                {
                    ModelState.Remove("SendTypeID");
                    ModelState.AddModelError("SendTypeID", "نحوه ارسال را انتخاب نمایید");

                }



                return View(vmshipping);
            }


            var statusID = 1;
            if (vmshipping.PaymentTypeid == 2)
                statusID = 7;
            var TrackingID = Helpers.CreateHash.RNGCharacterMask();
            TotalPrice += (long)vmshipping.SendTypePrice;
            TotalPrice += (long)VatPrice;
            var neworder = new Order()
            {

                Address = vmshipping.Address,
                MemberID = currentuser.UserId,
                Mobile = vmshipping.DeliverMobile,
                Name = vmshipping.DeliverName,
                OrderStatusID = statusID,

                PaymentType = vmshipping.PaymentTypeid,
                SendTypeID = vmshipping.SendTypeID,
                Tel = vmshipping.DeliverTel,
                TrackingID = TrackingID,
                SendPrice = (long)vmshipping.SendTypePrice,
                TotalPrice = TotalPrice
                ,
                OrderDate = DateTime.Now,
                Vat = (long)VatPrice,
                PstalCode = vmshipping.PostalCode,
                StateID = vmshipping.StateID,
                CityID = vmshipping.CityID


            };

            repOrder.Insert(neworder);
            var orderID = repOrder.getLastOrderID();
            foreach (var item in cartitems)
            {
                var newOrderProduct = new OrderProduct()
                {
                    OrderID = orderID,
                    ProductID = item.Product.Id,
                    Price = (long)item.Product.Price,
                    Quantity = item.Quantity
                };
                repOP.Insert(newOrderProduct);
            }


            double profitprice = ((Productsprice * (int)SiteSetting.ProfitPercent) / 100);
            if (currentuserInfo.ReagentCode > 0)
            {
                var firstuserinfo = db.UserInfoes.Where(c => c.id == currentuserInfo.ReagentCode).FirstOrDefault();
                if (firstuserinfo != null)
                {
                    var userbag = db.UserBags.Where(c => c.userId == firstuserinfo.UserId);
                    var firstbagprice = userbag.FirstOrDefault().BagPrice + profitprice;
                    userbag.FirstOrDefault().BagPrice = (long)firstbagprice;
                    db.Database.ExecuteSqlCommand("update UserBag set BagPrice=" + firstbagprice + " where userId='" + firstuserinfo.UserId + "'");
                    var newtrans = new BagTransaction() {
                        AddPrice =(long) profitprice,
                          BagId=userbag.FirstOrDefault().id,
                           dateInsert=DateTime.Now,
                            Descriptions="درصد خرید کاربر زیر مجموعه " + currentuserInfo.Name + " به مبلغ  " + Productsprice,
                             MinusPrice=0,
                             
                    };
                    db.BagTransactions.Add(newtrans);
                    db.SaveChanges();
                    var firstuserinfo2 = db.UserInfoes.Where(c => c.id == firstuserinfo.ReagentCode).FirstOrDefault();
                    if (firstuserinfo2 != null)
                    {
                        var userbag2 = db.UserBags.Where(c => c.userId == firstuserinfo2.UserId);
                        var firstbagprice2 = userbag2.FirstOrDefault().BagPrice + profitprice;
                        userbag2.FirstOrDefault().BagPrice = (long)firstbagprice2;
                        db.Database.ExecuteSqlCommand("update UserBag set BagPrice=" + firstbagprice2 + " where userId='" + firstuserinfo2.UserId + "'");
                        
                        db.BagTransactions.Add(newtrans);
                        db.SaveChanges();
                        var firstuserinfo3 = db.UserInfoes.Where(c => c.id == firstuserinfo2.ReagentCode).FirstOrDefault();
                        if (firstuserinfo3 != null)
                        {
                            var userbag3 = db.UserBags.Where(c => c.userId == firstuserinfo3.UserId);
                            var firstbagprice3 = userbag.FirstOrDefault().BagPrice + profitprice;
                            userbag3.FirstOrDefault().BagPrice = (long)firstbagprice3;
                            db.Database.ExecuteSqlCommand("update UserBag set BagPrice=" + firstbagprice3 + " where userId='" + firstuserinfo3.UserId + "'");
                          
                            db.BagTransactions.Add(newtrans);
                            db.SaveChanges();
                        }
                    }
                }


            }
            if (this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"] != null)
            {
                HttpCookie ShoppingCartCoolkie = this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"];

                ShoppingCartCoolkie.Expires = DateTime.Now.AddDays(-10);
                ShoppingCartCoolkie.Value = null;
                HttpContext.Response.SetCookie(ShoppingCartCoolkie);
            }





            return RedirectToAction("factor", new { id = orderID });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetCityList(int StateID)
        {
            var cityList = repCity.getByGroupID(StateID);
            List<VMCity> vmlist = new List<VMCity>();
            vmlist.Add(new VMCity() { Id = 0, Title = "انتخاب شهر" });

            foreach (var item in cityList)
            {
                vmlist.Add(item.toVMCity());
            }
            return Json(vmlist);
        }

        public ActionResult Factor(int? id)
        {
            if (id == null)
                return RedirectToAction("index", "home");
            if ((int)id <= 0)
            {
                return RedirectToAction("index", "Home");
            }

            var factor = repOrder.GetDapperByID((int)id);
            if (factor == null)
            {
                return RedirectToAction("index", "Home");
            }

            ViewBag.OrderInfo = factor;
            var activefactor = repsitsetting.GetAll().FirstOrDefault();
            var FactorProducts = repOP.GetDapperOrderID((int)id);
            if (activefactor.FactorActive == true)
            {
                return View("Factor", FactorProducts);
            }
            else
            {
                return View("TrackingCode", FactorProducts);
            }

        }
    }
}