using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MusicPlan.DAL.Repository
{
    public class ArtCollegeGenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
        public IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new ArtCollegeContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();

                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include(navigationProperty));

                list = dbQuery.AsNoTracking().ToList();
            }
            return list;
        }

        public IList<T> GetList(Func<T, bool> whereClause, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = new ArtCollegeContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<T> dbQuery = context.Set<T>();
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include(navigationProperty));

                list = dbQuery
                    .AsNoTracking()
                    .Where(whereClause)
                    .ToList();
            }
            return list;
        }

        public T GetSingle(Func<T, bool> whereClause, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item;
            using (var context = new ArtCollegeContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include(navigationProperty));

                item = dbQuery
                    .AsNoTracking()
                    .FirstOrDefault(whereClause);
            }
            return item;
        }

        public void Add(params T[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    context.Entry(item).State = EntityState.Added;
                }
                context.SaveChanges();
            }
        }

        public void Update(params T[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    context.Entry(item).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public void Remove(params T[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}
