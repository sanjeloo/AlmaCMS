using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;


namespace AlmaCMS.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext db;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DbContext context)
        {
            this.db = context;
            this.dbSet = context.Set<TEntity>();
        }

        public TEntity FindById(int id)
        {
            return dbSet.Find(id);
        }


        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            db.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            TEntity entityToDelete = dbSet.Find(id);

            if (db.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
            db.SaveChanges();
        }

        public IEnumerable<TEntity> Where(Func<TEntity, bool> expression)
        {
            List<TEntity> entityList = new List<TEntity>();
            foreach (var entity in dbSet)
            {
                if (expression(entity))
                {
                    entityList.Add(entity);
                }
            }
            return entityList;
        }
    }
}