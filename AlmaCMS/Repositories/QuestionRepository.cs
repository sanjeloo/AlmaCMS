using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class QuestionRepository : GenericRepository<Question>
    {
        public QuestionRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

    }
}