using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class ProjectRepository:GenericRepository<Project>
    {
        public ProjectRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        public List<Project> getByGroupID(int Groupid)
        {
            return   Where(c=>c.GroupID==Groupid).OrderByDescending(c=>c.id).ToList();
        }
    }
}