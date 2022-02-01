using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AlmaCMS.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        T FindById(int id);
        IEnumerable<T> Where(Func<T, bool> expression);

        IEnumerable<T> GetAll();

        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}