using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class ArticleRepository:GenericRepository<Article>
    {
        public ArticleRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public List<Article> getByGroupID(int Groupid)
        {
            return   Where(c=>c.ArticleGroupID==Groupid).OrderByDescending(c=>c.id).ToList();
        }
    }
}