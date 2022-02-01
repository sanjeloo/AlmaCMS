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
    public class ProductReportRepository: GenericRepository<ProductReport>
    {
        public ProductReportRepository(DB_AlmaCmsEntities db)
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

        public int getLastid()
        {
            var lastId= GetAll().Max(c => c.id);



            return lastId;

                     }
        public IEnumerable<VWProducReport> GetDapperReportList()
        {

            return Connection.Query<VWProducReport>("select * from  VWProducReport", null, commandType: CommandType.Text);


        }

        public IEnumerable<VWExpertReport> GetDapperExpertReportList()
        {

            return Connection.Query<VWExpertReport>("select * from  VWExpertReports", null, commandType: CommandType.Text);


        }
        public IEnumerable<VWProductReportExpert> GetDapperReportUsers(int taskid)
        {

            return Connection.Query<VWProductReportExpert>("select * from  VWProductReportExpert where ProductReportId=" + taskid, null, commandType: CommandType.Text);


        }
  
    }
}