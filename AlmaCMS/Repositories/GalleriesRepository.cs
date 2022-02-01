using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class GalleriesRepository : GenericRepository<Gallery>
    {
        public GalleriesRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }


        public IEnumerable<Gallery> GetAllByAlbumeID(int AlbumeID)
        {
            return Where(c => c.AlbumId == AlbumeID);
        }
    }
  
}