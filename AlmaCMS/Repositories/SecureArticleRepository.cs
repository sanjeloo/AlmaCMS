﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;

namespace AlmaCMS.Repositories
{
    public class SecureArticleRepository:GenericRepository<SecureArticle>
    {
        public SecureArticleRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
       
    }
}