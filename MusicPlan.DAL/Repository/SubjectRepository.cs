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
            List<Subject> list;
            using (var context = new ArtCollegeContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<Subject> dbQuery = context.Set<Subject>();

                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include(navigationProperty));

                list = dbQuery.AsNoTracking().ToList();
            }
            return list;
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
                        var existedParam = context.ParameterTypes.Local.SingleOrDefault(p => p.Id == param.Type.Id);

                        if (existedParam == null)
                        {
                            context.Entry(param.Type).State = EntityState.Unchanged;
                        }
                        else
                        {
                            param.Type = existedParam;
                        }
                        context.Entry(param).State = param.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                    }
                    context.Entry(item).State = EntityState.Added;
                }
                context.SaveChanges();
            }
        }

        public void Update(params Subject[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    var origItem = context.Subjects.Include(la => la.HoursParameters).Include(la => la.HoursParameters.Select(la1=>la1.Type)).SingleOrDefault(la => la.Id == item.Id);
                    context.Entry(origItem).CurrentValues.SetValues(item);
                    foreach (var param in item.HoursParameters)
                    {
                        var existedParam = context.ParameterTypes.Local.SingleOrDefault(p => p.Id == param.Type.Id);
                        param.Subject = null;

                        if (existedParam == null)
                        {
                            context.Entry(param.Type).State = EntityState.Unchanged;
                        }
                        else
                        {
                            param.Type = existedParam;
                        }

                        var origParamItem = context.SubjectsParameters.SingleOrDefault(la => la.Id == param.Id);
                        if (origParamItem != null)
                        {
                            context.Entry(origParamItem).CurrentValues.SetValues(param);
                            origParamItem.Type = param.Type;
                        }
                        else
                        {
                            //context.Entry(param).State = EntityState.Added;
                            origItem.HoursParameters.Add(param);
                        }
                        
                        //
                    }
                    //context.SubjectsParameters.Attach()
                    //context.Entry(item).State = EntityState.Modified;
                }
               // bool a = context.ChangeTracker.HasChanges();
                context.SaveChanges();
            }
        }

        public void Remove(params Subject[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    foreach (var param in item.HoursParameters)
                    {
                        var existedParamType = context.ParameterTypes.Local.SingleOrDefault(p => p.Id == param.Type.Id);
                        if (existedParamType == null)
                        {
                            context.Entry(param.Type).State = EntityState.Unchanged;
                        }
                        else
                        {
                            param.Type = existedParamType;
                        }
                        param.Subject = null;
                    }
                    context.Subjects.Attach(item);
                    context.Subjects.Remove(item);
                }
                context.SaveChanges();
            }
        }
    }
}
