using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
using AlmaCMS.Repositories;
using AlmaCMS.Helpers;
using PagedList;
using PagedList.Mvc;

using System.Web.Routing;
using System.Web.Security;

namespace AlmaCMS.Areas.Manage.Controllers
{

    [Authorize(Roles="Admin")]

    [HasPermission("ManageOrders")]
    public class ManageOrdersController : Controller
    {
        // GET: AdminShop/ManageOrders

        DB_AlmaCmsEntities db;

        OrderStatusRepository repstatus;
        UserInfoRepositiry repMembers;
        SendTypeRepository repSendType;
        OrdersRepository repOrder;
        OrderProductRepository repOP;
        public ManageOrdersController()
        {
            db = new DB_AlmaCmsEntities();
            repOrder = new OrdersRepository(db);
            repstatus = new OrderStatusRepository(db);
            repSendType = new SendTypeRepository(db);
            repMembers = new UserInfoRepositiry(db);

            repOP = new OrderProductRepository(db);
        }
        //
        // GET: /Manage/ManageOrders/
        public ActionResult Index()
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMembers.GetByUserID(User.Identity.getuser).MemberID, "ManageOrders"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}
            var orderlist = repOrder.GetDapperOrderListAll();
            ViewBag.StatusList = repstatus.GetAll();
            return View(orderlist);
        }
        public ActionResult ChangeState(int statusID, int OrderID)
        {

            var currentOrder = repOrder.FindById(OrderID);
            currentOrder.OrderStatusID = statusID;
            repOrder.Update(currentOrder);
            return RedirectToAction("index");
        }
        #region Delete

        public ActionResult Delete(int? id)
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMembers.GetByEmail(User.Identity.Name).MemberID, "DeleteOrder"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMOrderList order = repOrder.GetDapperByID((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.StatusList = repstatus.GetAll();
            return View(order);
        }

        public ActionResult Details(int? id)
        {
            //if (!NSShop.Helpers.UserRole.IsUserRole(repMembers.GetByEmail(User.Identity.Name).MemberID, "DetailOrder"))
            //{
            //    AdminController.locked = true;
            //    return RedirectToAction("Dashboard", "Admin");

            //}
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VMOrderList order = repOrder.GetDapperByID((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.OrderInfo = order;
            var FactorProducts = repOP.GetDapperOrderID((int)id);

            ViewBag.StatusList = repstatus.GetAll();
            return View(FactorProducts);
        }
        public JsonResult ShowOffersList(int productid)
        {
            //var vmofferslist = new List<VMProductOffersPriceSupplier>();
            //var offerslist = repOrder.GetProductsOffersByProductId(productid);
            try
            {

                //return Json(new { status = "Success", offerslist });

                return Json("Error");
            }
            catch (Exception exc)
            {
                return Json("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {


            repOrder.Delete(id);

            return RedirectToAction("Index");
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult GetList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, string FilterTitle = "", string TrackingCode = "", string FactorNo = "", string Name = "")
        {
            try
            {

                string strFilterStatus = "";
                string strCriteria = "";
                if (string.IsNullOrEmpty(jtSorting))
                {
                    jtSorting = "id DESC";
                }

                if (jtSorting == "PersianOrderDate ASC")
                {
                    jtSorting = "OrderDate ASC";
                }
                else if (jtSorting == "PersianOrderDate DESC")
                {
                    jtSorting = "OrderDate DESC";
                }
                if (FilterTitle != "0")
                {
                    strFilterStatus = " And OrderStatusID=" + FilterTitle;
                }

                if (!String.IsNullOrEmpty(TrackingCode))
                {

                    strCriteria += " And TrackingID='" + TrackingCode + "' ";

                }

                if (!String.IsNullOrEmpty(FactorNo))
                {

                    strCriteria += " And id='" + FactorNo + "' ";

                }


                if (!String.IsNullOrEmpty(Name))
                {

                    strCriteria += " And Name like N'%" + Name + "%' ";

                }
                var orderList = repOrder.GetDapperOrderList(jtStartIndex, jtPageSize, jtSorting, strFilterStatus, strCriteria);
                int totalrecords = repOrder.GetDapperOrderCount();


                foreach (var item in orderList)
                {
                    item.PersianOrderDate = item.OrderDate.ToPersianShortDate() + " " + item.OrderDate.ToShortTimeString();
                }
                var jsonResult = Json(new { Result = "OK", Records = orderList, TotalRecordCount = totalrecords }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
    }
}