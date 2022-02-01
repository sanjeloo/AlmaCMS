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
using Microsoft.Owin.Security;

namespace AlmaCMS.Helpers
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class HasPermissionAttribute : ActionFilterAttribute
    {
        
        private string _permission;

        public HasPermissionAttribute(string permission = null)
        {
            _permission = permission;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string loginURL = "/manage/admin/noaccess";



            var db = new Models.DB_AlmaCmsEntities();
            string useriod=HttpContext.Current.User.Identity.GetUserId();
            var RoleList = db.AdminRoles.Where(c => c.UserId == useriod);
            var CurrentRole=db.Roles.Where(c=>c.RoleName==_permission).FirstOrDefault();
            if(CurrentRole==null)
            {
                 filterContext.HttpContext.Response.Redirect(loginURL, true);
            }

            if(!RoleList.Select(c=>c.RoleId).Contains(CurrentRole.Id))
            {
                filterContext.HttpContext.Response.Redirect(loginURL, true);
            }


        }
    }
}