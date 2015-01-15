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
    public class StudentRepository : IGenericDataRepository<Student>
    {
        public IList<Student> GetAll(params Expression<Func<Student, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<Student> GetList(Func<Student, bool> @where, params Expression<Func<Student, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public Student GetSingle(Func<Student, bool> @where, params Expression<Func<Student, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Add(params Student[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    context.Entry(item).State = EntityState.Added;
                    foreach (var instrument in item.Instruments)
                    {
                        context.Entry(instrument).State = instrument.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                    }
                }
                context.SaveChanges();
            }
        }

        public void Update(params Student[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    var originalItem = context.Students.SingleOrDefault(la => la.Id == item.Id);
                    if (originalItem != null)
                    {

                        var lstInstrToRemove =
                            originalItem.Instruments.Where(
                                origInstrument => item.Instruments.All(la => la.Id != origInstrument.Id)).ToList();

                        var lstInstrToAdd =
                            item.Instruments.Where(newItem => originalItem.Instruments.All(la => la.Id != newItem.Id))
                                .ToList();

                        foreach (var instrumentToRemove in lstInstrToRemove)
                        {
                            originalItem.Instruments.Remove(instrumentToRemove);
                        }

                        foreach (var instrumentToAdd in lstInstrToAdd)
                        {
                            originalItem.Instruments.Add(instrumentToAdd);
                        }

                        item.Instruments.Clear();
                        context.Entry(item).State = EntityState.Modified;
                    }
                    
                }
                context.SaveChanges();
            }
        }

        public void Remove(params Student[] items)
        {
            throw new NotImplementedException();
        }
    }
}
