using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {
        /**
         * [TT]: Must explicitly mark the parameter with the nullable annotation and assign a null value.
         */
        IEnumerable<T> GetAll(IEnumerable<string>? includeProperties = null, Expression<Func<T, bool>>? filter = null);
        T Get(Expression<Func<T, bool>> filter, IEnumerable<string>? includeProperties = null, bool tracked = true);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(T entity);
    }
}
