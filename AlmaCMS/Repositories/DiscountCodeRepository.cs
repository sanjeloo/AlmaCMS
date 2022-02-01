using AlmaCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.Repositories
{
    public class DiscountCodeRepository : GenericRepository<DiscountCode>
    {
        public DiscountCodeRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
    }
}