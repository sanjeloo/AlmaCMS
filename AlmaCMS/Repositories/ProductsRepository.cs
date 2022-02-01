using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class ProductsRepository:GenericRepository<Product>
    {
        public ProductsRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public List<Product> getByGroupID(int Groupid)
        {
            return   Where(c=>c.GroupID==Groupid).OrderByDescending(c=>c.id).ToList();
        }

        public void AddVisitCount(int ProductId)
        {
            var Currentnews = FindById(ProductId);
            int Visitcount = (int)Currentnews.VisitCount + 1;
            db.Database.ExecuteSqlCommand("Update Products set VisitCount=" + Visitcount + " where id=" + ProductId);
        }
    }
}