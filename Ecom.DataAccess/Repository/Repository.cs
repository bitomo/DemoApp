using Ecom.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecom.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> DbSet { get; set; }


        public Repository(ApplicationDbContext dbContext)
        {
            DbSet = dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, IEnumerable<string>? includeProperties = null, bool tracked = true)
        {
            IQueryable<T> dbSet;

            if (tracked) dbSet = DbSet;
            else dbSet = DbSet.AsNoTracking();

            IQueryable<T> dbSetBuild = dbSet.Where(filter);

            if (includeProperties != null && includeProperties.Count() > 0)
            {
                foreach (var property in includeProperties)
                {
                    dbSetBuild = dbSetBuild.Include(property);
                }
            }

            return dbSetBuild.FirstOrDefault();
        }

        /**
         * [TT]: Must explicitly mark the parameter with the nullable annotation and assign a null value.
         */
        public IEnumerable<T> GetAll(IEnumerable<string>? includeProperties = null, Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> dbSetBuild = DbSet;
            if (filter != null)
            {
                dbSetBuild = dbSetBuild.Where(filter);
            }

            if (includeProperties != null && includeProperties.Count() > 0)
            {
                foreach (var property in includeProperties)
                {
                    dbSetBuild = dbSetBuild.Include(property);
                }
            }

            return dbSetBuild.ToList();
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(T entity)
        {
            DbSet.RemoveRange(entity);
        }

    }
}
