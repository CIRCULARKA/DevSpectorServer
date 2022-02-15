using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace DevSpector.Domain
{
    public interface IRepository
    {
        IEnumerable<T> Get<T>(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string include = "") where T : class;


        T GetByID<T>(object key) where T : class;

        T GetSingle<T>(Expression<Func<T, bool>> filter = null, string include = "")
            where T : class;

        void Add<T>(T entity) where T : class;

        void Remove<T>(object key) where T : class;

        void Remove<T>(T entity) where T : class;

        void Update<T>(T entity) where T : class;

        void Save();
    }
}
