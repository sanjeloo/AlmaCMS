using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class AlbumeRepository : GenericRepository<Album>
    {
        public AlbumeRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }
    }
}