﻿using AlmaCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlmaCMS.Repositories
{
    public class ExpertInfoRepository : GenericRepository<ExpertInfo>
    {
        public ExpertInfoRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
    }
}