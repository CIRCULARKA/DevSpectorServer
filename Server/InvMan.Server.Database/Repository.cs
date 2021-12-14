using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using InvMan.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace InvMan.Server.Domain
{
    public class Repository
    {
        protected ApplicationDbContext _context;

        public virtual IEnumerable<T> Get<T>(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string include = ""
        ) where T : class
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            foreach (var prop in include.Split(new char[] { ',', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(prop);

            if (orderBy != null)
                orderBy(query);

            return query.ToList();
        }

        public virtual T GetByID<T>(object key) where T : class =>
            _context.Set<T>().Find(key);

        public virtual void Add<T>(T entity) where T : class =>
            _context.Set<T>().Add(entity);

        public virtual void Remove<T>(object key) where T : class =>
            Remove(_context.Set<T>().Find(key));

        public virtual void Remove<T>(T entity) where T : class
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _context.Set<T>().Attach(entity);

            _context.Remove(entity);
        }

        public virtual void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Save() =>
            _context.SaveChanges();
    }
}
