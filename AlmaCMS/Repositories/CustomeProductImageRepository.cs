using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class CustomeProductImageRepository:GenericRepository<CustomeProsuctsImage>
    {
        public CustomeProductImageRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public List<CustomeProsuctsImage> getByProductID(int ProductID)
        {
            return Where(c => c.ProductID == ProductID).OrderByDescending(c => c.id).ToList();
        }
    }
}