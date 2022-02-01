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
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository(DB_AlmaCmsEntities db)
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
        public List<VWPeyment> getbymemberid(string memberid)
        {
            return Connection.Query<VWPeyment>("select * from  VWPeyments where MemberID=N'" + memberid + "'", null, commandType: CommandType.Text).ToList();

        }
        public VWPeyment GetDapperByID(int id)
        {

            return Connection.Query<VWPeyment>("select * from  VWPeyments where id=" + id, null, commandType: CommandType.Text).FirstOrDefault();


        }

        public int GetDapperPaymentCount()
        {

            return Connection.Query<int>("select count(*) from  VWPeyments", null, commandType: CommandType.Text).Single();


        }

        public IEnumerable<VWPeyment> GetDapperPaymentList(int startindex, int pagesize, string sortField, string filterStatus, string strCriteria)
        {
            string strquery = "WITH CTE AS (select   ROW_NUMBER() OVER ( ORDER BY " + sortField + " ) AS RowNum ,* from VWPeyments where 1=1  " + strCriteria + filterStatus + " ) select * from cte where (RowNum > " + startindex + " And RowNum<= " + (startindex + pagesize) + " ) ";




            if (filterStatus != "")
            {
                strquery = strquery;
            }
            strquery = strquery + " ORDER BY  " + sortField + "";

            return Connection.Query<VWPeyment>(strquery, null, commandType: CommandType.Text);

        }



    }
}