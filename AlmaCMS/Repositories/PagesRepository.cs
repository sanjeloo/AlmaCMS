using AlmaCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.Repositories
{
    public class PagesRepository : GenericRepository<Page>
    {

        public PagesRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public IEnumerable<Page> GetByParentID(int ParentID)
        {
            return Where(c => c.ParentID == ParentID);
        }
    }
}