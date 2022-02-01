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
    public class OrdersRepository : GenericRepository<Order>
    {
        public OrdersRepository(DB_AlmaCmsEntities db)
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
        public IEnumerable<VMOrderList> getbyorderid()
        {

            return Connection.Query<VMOrderList>("SP_OrderList", null, commandType: CommandType.StoredProcedure);
        }

        public List<Order> getbymemberid(string memberid)
        {
            return Where(c => c.MemberID == memberid).ToList();

        }

        public int getLastOrderID()
        {


            return Connection.Query<int>("select max(id) from Orders ", null, commandType: CommandType.Text).FirstOrDefault();

            ;
        }
        public IEnumerable<VMOrderList> GetDapperOrderListAll()
        {
            string strquery = "select * from VMOrderList  ";





            return Connection.Query<VMOrderList>(strquery, null, commandType: CommandType.Text);

        }
        public IEnumerable<VMOrderList> GetDapperOrderList(int startindex, int pagesize, string sortField, string filterStatus, string strCriteria)
        {
            string strquery = "WITH CTE AS (select   ROW_NUMBER() OVER ( ORDER BY " + sortField + " ) AS RowNum ,* from VMOrderList ) select * from cte where (RowNum > " + startindex + " And RowNum<= " + (startindex + pagesize) + strCriteria + " ) ";




            if (filterStatus != "")
            {
                strquery = strquery + filterStatus;
            }
            strquery = strquery + " ORDER BY  " + sortField + "";

            return Connection.Query<VMOrderList>(strquery, null, commandType: CommandType.Text);

        }


        public VMOrderList GetDapperByID(int FactorNo)
        {

            return Connection.Query<VMOrderList>("select * from  VMOrderList where id=" + FactorNo, null, commandType: CommandType.Text).FirstOrDefault();


        }

        public IEnumerable<VMOrderList> GetDapperByMemberID(string MemberID)
        {

            return Connection.Query<VMOrderList>("select * from  VMOrderList where MemberID='" + MemberID + "'", null, commandType: CommandType.Text);


        }
        public int GetDapperOrderCount()
        {

            return Connection.Query<int>("select count(*) from  VMOrderList", null, commandType: CommandType.Text).Single();


        }
        //public IEnumerable<VMProductOffersPriceSupplier> GetProductsOffersByProductId(int productid)
        //{
        //    var p = new DynamicParameters();
        //    p.Add("ProductId", productid);
        //    return Connection.Query<VMProductOffersPriceSupplier>("GetSuppliersPriceList", p, commandType: CommandType.StoredProcedure);
        //}
        public List<Order> GetByStatusID(int id)
        {
            return Where(c => c.OrderStatusID == id).ToList();
        }


    }
}