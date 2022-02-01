using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class ProductsGroupRepository:GenericRepository<ProductsGroup>
    {
        public ProductsGroupRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

      

    }
}