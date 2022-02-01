using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class USerBagRepository: GenericRepository<UserBag>
    {
        public USerBagRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }
    }
}