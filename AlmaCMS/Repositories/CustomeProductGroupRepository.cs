﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class CustomeProductGroupRepository:GenericRepository<CustomeProductGroup>
    {
        public CustomeProductGroupRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }
    }
}