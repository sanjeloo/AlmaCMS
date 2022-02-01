using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class RolesRepository:GenericRepository<Role>
    {
        public RolesRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
    }
}