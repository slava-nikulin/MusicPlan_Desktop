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
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    foreach (var param in item.HoursParameters)
                    {
                        param.Subject = null;
                    }
                    context.Entry(item).State = EntityState.Added;
                    foreach (var param in item.HoursParameters)
                    {
                        context.Entry(param).State = param.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                    }
                }
                context.SaveChanges();
            }
        }

        public void Update(params Subject[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                var uniqueType = new List<SubjectParameterType>();
                foreach (var item in items)
                {
                    foreach (var param in item.HoursParameters)
                    {
                        param.Subject = null;

                        var existedParam = uniqueType.SingleOrDefault(p => p.Id == param.Type.Id);
                        if (existedParam == null)
                        {
                            context.Entry(param.Type).State = EntityState.Unchanged;
                            uniqueType.Add(param.Type);
                        }
                        else
                        {
                            param.Type = existedParam;
                        }
                        context.Entry(param).State = param.Id > 0 ? EntityState.Modified : EntityState.Added;
                    }
                    context.Entry(item).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public void Remove(params Subject[] items)
        {
            throw new NotImplementedException();
        }
    }
}
