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
    public class TeacherRepository : IGenericDataRepository<Teacher>
    {
        public IList<Teacher> GetAll(params Expression<Func<Teacher, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<Teacher> GetList(Func<Teacher, bool> @where, params Expression<Func<Teacher, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public Teacher GetSingle(Func<Teacher, bool> @where, params Expression<Func<Teacher, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Add(params Teacher[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    context.Entry(item).State = EntityState.Added;
                    foreach (var instrument in item.Subjects)
                    {
                        context.Entry(instrument).State = instrument.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                    }
                }
                context.SaveChanges();
            }
        }

        public void Update(params Teacher[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    var originalItem = context.Teachers.Include(la => la.Subjects).SingleOrDefault(la => la.Id == item.Id);
                    if (originalItem != null)
                    {
                        context.Entry(originalItem).CurrentValues.SetValues(item);
                        var lstNavItemsToRemove =
                            originalItem.Subjects.Where(
                                origNavItem => item.Subjects.All(la => la.Id != origNavItem.Id)).ToList();

                        var lstInstrToAdd =
                            item.Subjects.Where(newItem => originalItem.Subjects.All(la => la.Id != newItem.Id))
                                .ToList();

                        foreach (var navItemToRemove in lstNavItemsToRemove)
                        {
                            originalItem.Subjects.Remove(navItemToRemove);
                        }

                        foreach (var navItemToAdd in lstInstrToAdd)
                        {
                            originalItem.Subjects.Add(navItemToAdd);
                        }

                        foreach (var navItem in originalItem.Subjects)
                        {
                            context.Entry(navItem).State = navItem.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                        }
                    }

                }
                context.SaveChanges();
            }
        }

        public void Remove(params Teacher[] items)
        {
            throw new NotImplementedException();
        }
    }
}
