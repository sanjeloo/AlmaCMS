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
    public class ProductReviewRepository : GenericRepository<ProductReview>
    {
        public ProductReviewRepository(DB_AlmaCmsEntities db)
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
        public List<ProductReview> getByProductID(int ProductID)
        {
            return Where(c => c.productid == ProductID).OrderByDescending(c => c.DateInsert).ToList();
        }
        public IEnumerable<VMReviews> GetReviews()
        {

            return Connection.Query<VMReviews>("SP_GetReviews", null, commandType: CommandType.StoredProcedure);
        }
    }
}