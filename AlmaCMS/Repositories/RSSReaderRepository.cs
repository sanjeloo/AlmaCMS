using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class RSSReaderRepository:GenericRepository<RssReader>
    {
        public RSSReaderRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
    }
}