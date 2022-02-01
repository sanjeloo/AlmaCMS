using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace AlmaCMS.Repositories
{
    public class AspUserRepository : GenericRepository<AspNetUser>
    {
        public AspUserRepository(DB_AlmaCmsEntities db)
            : base(db)
        {
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }
        public IEnumerable<ViewModels.Expert.VMExpertInfo> getExpertInfo()
        {
            return Connection.Query<ViewModels.Expert.VMExpertInfo>("select * from  VWExpertInfo", null, commandType: CommandType.Text).AsEnumerable();
        }





    }
}