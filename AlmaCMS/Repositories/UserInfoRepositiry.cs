using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using AlmaCMS.Models;
using AlmaCMS.ViewModels;
namespace AlmaCMS.Repositories
{
    public class UserInfoRepositiry : GenericRepository<UserInfo>
    {
        public UserInfoRepositiry(DB_AlmaCmsEntities db)
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
        public UserInfo GetByUserID(string UserID)
        {
            return Where(c => c.UserId == UserID).FirstOrDefault();
        }


        public IEnumerable<VWUserInfo> getDapperUserInfo()
        {

            return Connection.Query<VWUserInfo>("select * from  VWUserInfo ", null, commandType: CommandType.Text);


        }

  


    }
}