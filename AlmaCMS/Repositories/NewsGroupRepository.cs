using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class NewsGroupRepository:GenericRepository<NewsGroup>
    {
        public NewsGroupRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }
        
       
    }
}