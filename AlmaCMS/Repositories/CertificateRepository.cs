using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class CertificateRepository:GenericRepository<Certificate>
    {
        public CertificateRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }
        public List<Certificate> getByGroupID(int Groupid)
        {
            return Where(c => c.GroupID == Groupid).OrderByDescending(c => c.id).ToList();
        }
    }
}