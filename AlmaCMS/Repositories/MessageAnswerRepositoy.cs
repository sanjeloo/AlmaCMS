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
    public class MessageAnswerRepositoy: GenericRepository<MessageAnswer>
    {
        public MessageAnswerRepositoy(DB_AlmaCmsEntities db)
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


        public IEnumerable<VWMessageAnswer> GetDapperAnswerList()
        {

            return Connection.Query<VWMessageAnswer>("select * from  VWMessageAnswer", null, commandType: CommandType.Text);


        }
    }
}
