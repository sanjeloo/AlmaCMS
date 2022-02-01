using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Repositories;
using AlmaCMS.ViewModels;
using AlmaCMS.Models;
using AlmaCMS.Helpers;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace AlmaCMS.Controllers
{

    // GET: Cart
    public class CartController : Controller
    {
        // GET: Cart

        ProductsRepository RepProduct;
        DB_AlmaCmsEntities db;
        public CartController()
        {
            db = new DB_AlmaCmsEntities();
            RepProduct = new ProductsRepository(db);
        }
        public ActionResult Index()
        {

            List<VMCartItem> cartitems = new List<VMCartItem>();
            if (this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"] != null)
            {
                HttpCookie ShoppingCartCoolkie = this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"];

                var jsCartItems = Server.UrlDecode(ShoppingCartCoolkie.Value.ToString());
                cartitems = JsonConvert.DeserializeObject<List<VMCartItem>>(jsCartItems);
            }
            return View(cartitems);

        }


        public ActionResult DeleteItem(int id)
        {
            try
            {


                List<VMCartItem> cartitems = new List<VMCartItem>();
                HttpCookie ShoppingCartCoolkie = this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"];
                var jsCartItems = Server.UrlDecode(ShoppingCartCoolkie.Value.ToString());
                cartitems = JsonConvert.DeserializeObject<List<VMCartItem>>(jsCartItems);

                var removeitem = cartitems.Where(c => c.Product.Id == id).FirstOrDefault();

                if (removeitem != null)
                {
                    cartitems.Remove(removeitem);

                }


                HttpCookie shoppingcartCookie = new HttpCookie("NSShopCartGLB");
                string strShoppingCart = new JavaScriptSerializer().Serialize(Json(cartitems).Data);
                shoppingcartCookie.Value = Server.UrlEncode(strShoppingCart);
                Response.SetCookie(shoppingcartCookie);
                return Json("1");
            }
            catch (Exception ex)
            {
                return Json("-1");
            }
        }

        public ActionResult UpdateItem(int id, int Quantity)
        {
            try
            {


                List<VMCartItem> cartitems = new List<VMCartItem>();
                HttpCookie ShoppingCartCoolkie = this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"];
                var jsCartItems = Server.UrlDecode(ShoppingCartCoolkie.Value.ToString());
                cartitems = JsonConvert.DeserializeObject<List<VMCartItem>>(jsCartItems);

                var UpdateItem = cartitems.Where(c => c.Product.Id == id).FirstOrDefault();

                if (UpdateItem != null)
                {
                    UpdateItem.Quantity = Quantity;

                }


                HttpCookie shoppingcartCookie = new HttpCookie("NSShopCartGLB");
                string strShoppingCart = new JavaScriptSerializer().Serialize(Json(cartitems).Data);
                shoppingcartCookie.Value = Server.UrlEncode(strShoppingCart);
                Response.SetCookie(shoppingcartCookie);
                return Json("1");
            }
            catch (Exception ex)
            {
                return Json("-1");
            }
        }

        public ActionResult AddToCart(int id)
        {

            try
            {

                var currentprod = RepProduct.FindById(id);
                var cartProduct = new VMCartProduct()
                {
                    Id = currentprod.id,
                    Image = currentprod.Image,
                    PartNumber = currentprod.id.ToString(),
                    Price = (double)currentprod.price,
                    Title = currentprod.Title,
                   


                };
                List<VMCartItem> cartitems = new List<VMCartItem>();
                if (this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"] == null)
                {


                    cartitems.Add(new VMCartItem(cartProduct, 1));
                    HttpCookie shoppingcartCookie = new HttpCookie("NSShopCartGLB");
                    string strShoppingCart = new JavaScriptSerializer().Serialize(Json(cartitems).Data);
                    shoppingcartCookie.Value = Server.UrlEncode(strShoppingCart);
                    shoppingcartCookie.Expires = DateTime.Now.AddDays(2);
                    Response.SetCookie(shoppingcartCookie);
                }
                else
                {
                    HttpCookie ShoppingCartCoolkie = this.ControllerContext.HttpContext.Request.Cookies["NSShopCartGLB"];
                    var jsCartItems = Server.UrlDecode(ShoppingCartCoolkie.Value.ToString());
                    cartitems = JsonConvert.DeserializeObject<List<VMCartItem>>(jsCartItems);

                    var existItem = (cartitems.Where(c => c.Product.Id == id).FirstOrDefault());
                    {
                        if (existItem == null)
                        {
                            cartitems.Add(new VMCartItem(cartProduct, 1));
                        }
                        else
                        {
                            cartitems.Where(c => c.Product.Id == id).FirstOrDefault().Quantity++;
                        }
                    }
                    HttpCookie shoppingcartCookie = new HttpCookie("NSShopCartGLB");
                    string strShoppingCart = new JavaScriptSerializer().Serialize(Json(cartitems).Data);
                    shoppingcartCookie.Value = Server.UrlEncode(strShoppingCart);
                    shoppingcartCookie.Expires = DateTime.Now.AddDays(2);
                    Response.SetCookie(shoppingcartCookie);

                }
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("index");
            }

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
