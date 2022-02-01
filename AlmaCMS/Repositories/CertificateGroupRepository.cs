using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class CertificateGroupRepository:GenericRepository<CerticicateGroup>
    {
        public CertificateGroupRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
    }
}