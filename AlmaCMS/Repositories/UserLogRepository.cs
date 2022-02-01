using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class UserLogRepository:GenericRepository<UserLog>
    {
        public UserLogRepository(DbContext context)
            :base(context)
        {

        }
        public List<UserLog> GetLogByUserId(int userId)
        {
            return Where(c => c.UserId == userId).ToList();
        }
    }
}