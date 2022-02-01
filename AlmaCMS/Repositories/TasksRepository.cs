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
    public class TasksRepository: GenericRepository<TasksList>
    {
        public TasksRepository(DB_AlmaCmsEntities db)
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
        public IEnumerable<VWTaskList> GetDapperTaskList()
        {

            return Connection.Query<VWTaskList>("select * from  VWTaskList", null, commandType: CommandType.Text);


        }

        public IEnumerable<VWExpertTask> GetDapperExpertTaskList()
        {

            return Connection.Query<VWExpertTask>("select * from  VWExpertTask", null, commandType: CommandType.Text);


        }
        public IEnumerable<VWTaskUser> GetDapperTaskUsers(int taskid)
        {

            return Connection.Query<VWTaskUser>("select * from  VWTaskUsers where taskid=" + taskid, null, commandType: CommandType.Text);
            

        }
    }
}