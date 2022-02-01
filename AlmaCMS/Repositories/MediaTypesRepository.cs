using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class MediaTypesRepository : GenericRepository<MediaType>
    {
        public MediaTypesRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
    }
}