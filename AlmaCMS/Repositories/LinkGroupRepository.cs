using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;


namespace AlmaCMS.Repositories
{
    public class LinkGroupRepository:GenericRepository<LinksGroup>
    {
        public LinkGroupRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }
    }
}