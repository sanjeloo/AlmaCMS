using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class SiteSettingRepository : GenericRepository<SiteSetting>
    {
        public SiteSettingRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }


    }
}