using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Helpers
{
    public class IsIdentity
    {
        public static bool Authorized()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Session["sb_m_m"].ToString()))
                {
                    string email = HttpContext.Current.Session["sb_m_m"].ToString();
                    var db = new DB_AlmaCmsEntities();
                    var info = db.Admins.Where(a => a.Email == email && a.IsActive == true).SingleOrDefault();
                    if (info != null)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }


        public static string GetName()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Session["sb_m_m"].ToString()))
                {
                    string email = HttpContext.Current.Session["sb_m_m"].ToString();
                    var db = new DB_AlmaCmsEntities();
                    var info = db.Admins.Where(a => a.Email == email && a.IsActive == true).SingleOrDefault();
                    if (info != null)
                        return info.Email;
                    else
                        return string.Empty;
                }
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }


        public static Dictionary<string, string> ReturnUrl()
        {
            string area = "";
            try
            {
                area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            }
            catch
            {
                area = "";
            }
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            string host = HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            var typeid = HttpContext.Current.Request.Params["TypeID"] ?? HttpContext.Current.Request.RequestContext.RouteData.Values["TypeID"] as string;
            string result = host + "/" + area + "/" + controller + "/" + action;
            Dictionary<string, string> url = new Dictionary<string, string>();
            url["area"] = area;
            url["controller"] = controller;
            url["action"] = action;
            url["typeid"] = typeid;
            return url;
        }
    }
}