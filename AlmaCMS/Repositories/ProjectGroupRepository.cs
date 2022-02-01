using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class ProjectGroupRepository:GenericRepository<ProjecGroup>
    {
        public ProjectGroupRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }
    }
}