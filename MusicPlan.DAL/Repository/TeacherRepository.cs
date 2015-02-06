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
                    foreach (var subject in item.Subjects)
                    {
                        subject.Teachers = null;
                        context.Entry(subject).State = subject.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                    }
                    context.Entry(item).State = EntityState.Added;
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
                            context.Subjects.Attach(navItemToRemove);
                            originalItem.Subjects.Remove(navItemToRemove);
                        }

                        foreach (var navItemToAdd in lstInstrToAdd)
                        {
                            context.Subjects.Attach(navItemToAdd);
                            originalItem.Subjects.Add(navItemToAdd);
                        }

                        //foreach (var navItem in originalItem.Subjects)
                        //{
                        //    navItem.Teachers = null;
                        //    context.Entry(navItem).State = navItem.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                        //}
                    }
                }
                context.SaveChanges();
            }
        }

        public void Remove(params Teacher[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    foreach (var subject in item.Subjects)
                    {
                        subject.Teachers = null;
                    }
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}
