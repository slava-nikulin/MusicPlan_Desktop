using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan.DAL.Repository
{
    public class SubjectRepository : IGenericDataRepository<Subject>
    {
        public IList<Subject> GetAll(params Expression<Func<Subject, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<Subject> GetList(Func<Subject, bool> @where, params Expression<Func<Subject, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public Subject GetSingle(Func<Subject, bool> @where, params Expression<Func<Subject, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Add(params Subject[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params Subject[] items)
        {
            throw new NotImplementedException();
        }

        public void Remove(params Subject[] items)
        {
            throw new NotImplementedException();
        }
    }
}
