using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class AdminRolesRepository:GenericRepository<AdminRole>
    {
        public AdminRolesRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public List<AdminRole> getByUserID(string UserId)
        {
            return Where(c => c.UserId == UserId).ToList();
        }
    }
}