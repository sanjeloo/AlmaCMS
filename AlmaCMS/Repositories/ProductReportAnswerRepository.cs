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
    public class ProductReportAnswerRepository: GenericRepository<ProductReportAnswer>
    {
        public ProductReportAnswerRepository(DB_AlmaCmsEntities db)
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


        public IEnumerable<VWProductReportAnswer> GetDapperAnswerList()
        {

            return Connection.Query<VWProductReportAnswer>("select * from  VWProductReportAnswer", null, commandType: CommandType.Text);


        }
    }
}