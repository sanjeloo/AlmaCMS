using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class CustomeProductsRepository:GenericRepository<CustomProduct>
    {
        public CustomeProductsRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public List<CustomProduct> getByGroupID(int Groupid)
        {
            return   Where(c=>c.GroupId==Groupid).OrderByDescending(c=>c.id).ToList();
        }
    }
}