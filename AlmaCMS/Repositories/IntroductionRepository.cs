using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class IntroductionRepository: GenericRepository<Introduction>
    {
        public IntroductionRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }
    }
}