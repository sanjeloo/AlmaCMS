using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class ProductsImageRepository:GenericRepository<ProductsImage>
    {
        public ProductsImageRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public List<ProductsImage> getByProductID(int ProductID)
        {
            return Where(c => c.ProductID == ProductID).OrderByDescending(c => c.id).ToList();
        }
    }
}