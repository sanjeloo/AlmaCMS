using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class OrderStatusRepository : GenericRepository<OrderStatus>
    {
        public OrderStatusRepository(DB_AlmaCmsEntities db)
            : base(db)
        { }
    }
}