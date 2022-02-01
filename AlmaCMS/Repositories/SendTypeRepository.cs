using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class SendTypeRepository : GenericRepository<SendType>
    {
        public SendTypeRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
    }
}