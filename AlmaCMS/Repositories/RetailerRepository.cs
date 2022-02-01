using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class RetailerRepository:GenericRepository<Retailer>
    {
        public RetailerRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public Retailer getByUserName(string UserName)
        {
            return Where(c => c.USerName == UserName && c.ActiveState==true).FirstOrDefault();
        }
    }
}