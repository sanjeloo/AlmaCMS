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
    public class OrderProductRepository : GenericRepository<OrderProduct>
    {
        public OrderProductRepository(DB_AlmaCmsEntities db)
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
        public IEnumerable<VMOrderProductSite> GetDapperOrderID(int OrderID)
        {

            return Connection.Query<VMOrderProductSite>("select * from  VWOrderProducts where OrderID=" + OrderID, null, commandType: CommandType.Text);


        }

    }
}