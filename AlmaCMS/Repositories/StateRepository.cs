using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class StateRepository : GenericRepository<State>
    {
        public StateRepository(DB_AlmaCmsEntities DB)
            : base(DB)
        {

        }
    }
}