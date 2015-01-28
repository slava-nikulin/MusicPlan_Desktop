using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan.DAL.Repository
{
    public class SubjectParametersRepository : IGenericDataRepository<SubjectParameters>
    {
        public IList<SubjectParameters> GetAll(params Expression<Func<SubjectParameters, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<SubjectParameters> GetList(Func<SubjectParameters, bool> @where, params Expression<Func<SubjectParameters, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SubjectParameters GetSingle(Func<SubjectParameters, bool> @where, params Expression<Func<SubjectParameters, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Add(params SubjectParameters[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params SubjectParameters[] items)
        {
            throw new NotImplementedException();
        }

        public void Remove(params SubjectParameters[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    item.Subject.HoursParameters = null;
                    context.Entry(item.Subject).State = EntityState.Unchanged;
                    context.Entry(item.Type).State = EntityState.Unchanged;
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}
