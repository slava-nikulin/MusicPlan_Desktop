using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MusicPlan.DAL.Repository
{
    /// <summary>
    /// Entity framework is not for working with derived context
    /// Take my advice - USE ADO.NET ;)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IGenericDataRepository<T> where T : class
    {
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        void Add(params T[] items);
        void Update(params T[] items);
        void Remove(params T[] items);
    }
}
