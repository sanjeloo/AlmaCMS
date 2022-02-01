using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlmaCMS.Models;
using System.Data;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using AlmaCMS.ViewModels;

namespace AlmaCMS.Repositories
{
    public class FavorisRepository : GenericRepository<Favorit>
    {
        public FavorisRepository(DB_AlmaCmsEntities db)
            : base(db)
        {

        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["DapperConnection"].ConnectionString);
            }
        }

        public IEnumerable<VMFavortis> GetByMemberId(string MemberId)
        {
            return Connection.Query<VMFavortis>("Select * From VWFavoritProduct Where MemberId='" + MemberId + "'", null, commandType: CommandType.Text);
        }

    }
}