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
    //public class UserMessageRepository: GenericRepository<UsersMessage>
    //{
    //    public UserMessageRepository(DB_AlmaCmsEntities db)
    //        : base(db)
    //    {
            
    //    }
    //    internal IDbConnection Connection
    //    {
    //        get
    //        {
    //            return new SqlConnection(ConfigurationManager.ConnectionStrings["DapperConnection"].ConnectionString);
    //        }
    //    }

    //    public int getLastid()
    //    {
    //        var lastId= GetAll().Max(c => c.id);



    //        return lastId;

    //                 }
    //    public IEnumerable<UsersMessage> GetDapperTaskList()
    //    {

    //        return Connection.Query<UsersMessage>("select * from  UsersMessages", null, commandType: CommandType.Text);


    //    }

    //    //public IEnumerable<VWUserMessage> GetDapperExpertTaskList()
    //    //{

    //    //    return Connection.Query<VWUserMessage>("select * from  VWUserMessages", null, commandType: CommandType.Text);


    //    //}
    //    public IEnumerable<VWMessageUser> GetDapperMessageUsers(int taskid)
    //    {

    //        return Connection.Query<VWMessageUser>("select * from  VWMessageUsers where taskid=" + taskid, null, commandType: CommandType.Text);
            

    //    }
    //}
}