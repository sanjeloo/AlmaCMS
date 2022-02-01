using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class ArticleGroupRepositry : GenericRepository<ArticleGroup>
    {
        public ArticleGroupRepositry(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }
    }
}