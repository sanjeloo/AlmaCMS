using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class NewsRepository : GenericRepository<News>
    {
        public NewsRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }



        public List<News> getByGroupID(int Groupid)
        {
            return   Where(c=>c.NewsGroupID==Groupid).OrderByDescending(c=>c.id).ToList();
        }



        public void AddVisitCount(int newsid)
        {
            var Currentnews = FindById(newsid);
            int Visitcount=(int)Currentnews.VisitCount+1 ;
            db.Database.ExecuteSqlCommand("Update news set visitcount=" + Visitcount  + " where id=" + newsid);
        }

    }
}