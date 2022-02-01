using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using AlmaCMS.Models;
namespace AlmaCMS.Helpers
{
    //public class CustomeRoleProvider : RoleProvider
    //{
    //    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override string ApplicationName
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public override void CreateRole(string roleName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override string[] GetAllRoles()
    //    {
    //        throw new NotImplementedException();
    //    }




    //    //public override string[] GetRolesForUser(string username)
    //    //{
    //    //    using (var db = new DB_AlmaCmsEntities())
    //    //    {
    //    //        Admin user = db.Admins.FirstOrDefault(e => e.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));

    //    //        var roles = from ur in user.AdminRoles
    //    //                    select ur.Role.RoleName;

    //    //        if (roles != null)
    //    //            return roles.ToArray();
    //    //        else
    //    //            return new string[] { };
    //    //    }
    //    //}

    //    //public override bool IsUserInRole(string username, string roleName)
    //    //{
    //    //    using (var db = new DB_AlmaCmsEntities())
    //    //    {
    //    //        Admin user = db.Admins.FirstOrDefault(e => e.Email.Equals(username));

    //    //        var roles = from ur in user.AdminRoles
    //    //                    from r in db.Roles
    //    //                    where ur.Id == r.Id
    //    //                    select r.RoleName;

    //    //        if (user != null)
    //    //            return roles.Any(e => e.Equals(username, StringComparison.CurrentCultureIgnoreCase));
    //    //        else
    //    //            return false;
    //    //    }
    //    //}




    //    public override string[] GetUsersInRole(string roleName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override bool RoleExists(string roleName)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}