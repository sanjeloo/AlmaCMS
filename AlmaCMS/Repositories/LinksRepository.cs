using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class LinksRepository : GenericRepository<Link>
    {
        public LinksRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public List<Link> getByGroupID(int Groupid)
        {
            return   Where(c=>c.LinkGroupID==Groupid).OrderByDescending(c=>c.id).ToList();
        }
    }
}