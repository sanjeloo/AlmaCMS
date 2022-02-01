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
  

    public class TaskAnsweRepository : GenericRepository<TaskAnswer>
    {
        public TaskAnsweRepository(DB_AlmaCmsEntities db)
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
            var lastId = GetAll().Max(c => c.id);



            return lastId;

        }
      
    }
}