using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class CityRepository : GenericRepository<City>
    {
        public CityRepository(DB_AlmaCmsEntities DB)
            : base(DB)
        {

        }

        public List<City> getByGroupID(int groupid)
        {
            return Where(c => c.StateID == groupid).OrderByDescending(c => c.Id).ToList();
        }

    }
}