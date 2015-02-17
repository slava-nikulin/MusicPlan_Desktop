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
    public class InstrumentRepositury : IGenericDataRepository<Instrument>
    {
        public IList<Instrument> GetAll(params Expression<Func<Instrument, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IList<Instrument> GetList(Func<Instrument, bool> @where, params Expression<Func<Instrument, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public Instrument GetSingle(Func<Instrument, bool> @where, params Expression<Func<Instrument, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Add(params Instrument[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params Instrument[] items)
        {
            throw new NotImplementedException();
        }

        public void Remove(params Instrument[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    var teacherBindings =
                                context.StudentsToTeachers.Where(la => la.Instrument.Id == item.Id);
                    foreach (var b in teacherBindings)
                    {
                        context.StudentsToTeachers.Remove(b);
                    }
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}
