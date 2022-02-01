using AlmaCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace AlmaCMS.Repositories
{
    public class MenuItemRepository : GenericRepository<MenuItem>
    {
        public MenuItemRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public IEnumerable<MenuItem> GetByParentID(int ParentID)
        {
            return Where(c => c.ParentID == ParentID);
        }

        public IEnumerable<MenuItem> GetByParentIDNonPrimary(int ParentID)
        {
            return Where(c => c.ParentID == ParentID && c.IsPrimary == false);
        }

        public MenuItem GetPriparyMenu()
        {
            return Where(c => c.IsPrimary == true).FirstOrDefault();
        }
    }
}