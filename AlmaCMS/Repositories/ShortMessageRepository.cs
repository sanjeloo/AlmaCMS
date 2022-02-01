using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
namespace AlmaCMS.Repositories
{
    public class ShortMessageRepository: GenericRepository<UsersShortMessage>
    {
        public ShortMessageRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
            
        }

        public List<UsersShortMessage> GetByUserId(string UserId)

        {
            return Where(c=>c.UserId==UserId).ToList();

        }
    }
}