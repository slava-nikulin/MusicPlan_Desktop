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

        public Student GetSingle(Func<Student, bool> whereClause, params Expression<Func<Student, object>>[] navigationProperties)
        {
            Student item;
            using (var context = new ArtCollegeContext())
            {
                context.Configuration.ProxyCreationEnabled = false;
                IQueryable<Student> dbQuery = context.Set<Student>();

                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include(navigationProperty));

                item = dbQuery
                    .AsNoTracking()
                    .FirstOrDefault(whereClause);
            }
            return item;
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
                    var originalItem = context.Students.Include(la => la.Instruments).SingleOrDefault(la => la.Id == item.Id);
                    if (originalItem != null)
                    {
                        context.Entry(originalItem).CurrentValues.SetValues(item);

                        //Instruments bindings
                        var lstInstrToRemove =
                            originalItem.Instruments.Where(
                                origInstrument => item.Instruments.All(la => la.Id != origInstrument.Id)).ToList();

                        var lstInstrToAdd =
                            item.Instruments.Where(newItem => originalItem.Instruments.All(la => la.Id != newItem.Id))
                                .ToList();

                        foreach (var instrumentToRemove in lstInstrToRemove)
                        {
                            originalItem.Instruments.Remove(instrumentToRemove);
                            var teacherBindings =
                                context.StudentsToTeachers.Where(la => la.Instrument.Id == instrumentToRemove.Id);
                            foreach (var b in teacherBindings)
                            {
                                item.StudentToTeachers.Remove(item.StudentToTeachers.SingleOrDefault(la => la.Id == b.Id));
                                context.StudentsToTeachers.Remove(b);
                            }
                        }

                        foreach (var instrumentToAdd in lstInstrToAdd)
                        {
                            var existedInstrument =
                                context.Instruments.Local.SingleOrDefault(la => la.Id == instrumentToAdd.Id);
                            if (existedInstrument != null)
                            {
                                originalItem.Instruments.Add(existedInstrument);
                            }
                            else
                            {
                                context.Entry(instrumentToAdd).State = instrumentToAdd.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                                originalItem.Instruments.Add(instrumentToAdd);
                            }
                        }

                        //Subject to teacher bindings
                        var lstSubjBindToRemove =
                            originalItem.StudentToTeachers.Where(origBind => item.StudentToTeachers.All(la => la.Id != origBind.Id)).ToList();

                        foreach (var subjBindToRemove in lstSubjBindToRemove)
                        {
                            context.StudentsToTeachers.Remove(subjBindToRemove);
                            item.StudentToTeachers.Remove(item.StudentToTeachers.SingleOrDefault(la => la.Id == subjBindToRemove.Id));
                        }

                        var lstSubjBindToAdd =
                            item.StudentToTeachers.Where(newItem => originalItem.StudentToTeachers.All(la => la.Id != newItem.Id))
                                .ToList();

                        foreach (var subjBindToAdd in lstSubjBindToAdd)
                        {
                            //instruments
                            var existedInstrument =
                                context.Instruments.Local.SingleOrDefault(p => p.Id == subjBindToAdd.Instrument.Id);
                            if (existedInstrument == null)
                            {
                                context.Entry(subjBindToAdd.Instrument).State = EntityState.Unchanged;
                            }
                            else
                            {
                                subjBindToAdd.Instrument = existedInstrument;
                            }
                            Teacher existedTeacher;
                            if (context.Subjects.Local.SingleOrDefault(p => p.Id == subjBindToAdd.Subject.Id) == null)
                            {
                                //parameters and types
                                SubjectParameterType existedType;
                                foreach (var parameter in subjBindToAdd.Subject.HoursParameters)
                                {
                                    parameter.Subject = null;
                                    if (context.SubjectsParameters.Local.SingleOrDefault(p => p.Id == parameter.Id) == null)
                                    {
                                        existedType = context.ParameterTypes.Local.SingleOrDefault(p => p.Id == parameter.Type.Id);
                                        if (existedType == null)
                                        {
                                            context.Entry(parameter.Type).State = EntityState.Unchanged;
                                        }
                                        else
                                        {
                                            parameter.Type = existedType;
                                        }
                                        context.Entry(parameter).State = EntityState.Unchanged;
                                    }
                                }
                                existedType = context.ParameterTypes.Local.SingleOrDefault(p => p.Id == subjBindToAdd.SubjectType.Id);
                                if (existedType == null)
                                {
                                    context.Entry(subjBindToAdd.SubjectType).State = EntityState.Unchanged;
                                }
                                else
                                {
                                    subjBindToAdd.SubjectType = existedType;
                                }
                                //teachers
                                var existedTeachers = new List<Teacher>();
                                foreach (var teacher in subjBindToAdd.Subject.Teachers)
                                {
                                    teacher.Subjects = null;
                                    existedTeacher = context.Teachers.Local.SingleOrDefault(p => p.Id == teacher.Id);
                                    if (existedTeacher == null)
                                    {
                                        context.Entry(teacher).State = EntityState.Unchanged;
                                    }
                                    else
                                    {
                                        existedTeachers.Add(existedTeacher);
                                    }
                                }
                                foreach (var t in existedTeachers)
                                {
                                    var teacherToRemove = subjBindToAdd.Subject.Teachers.Single(la => la.Id == t.Id);
                                    subjBindToAdd.Subject.Teachers.Remove(teacherToRemove);
                                    subjBindToAdd.Subject.Teachers.Add(t);
                                }
                                context.Entry(subjBindToAdd.Subject).State = EntityState.Unchanged;
                            }
                            existedTeacher = context.Teachers.Local.SingleOrDefault(p => p.Id == subjBindToAdd.Teacher.Id);
                            if (existedTeacher == null)
                            {
                                context.Entry(subjBindToAdd.Teacher).State = EntityState.Unchanged;
                            }
                            else
                            {
                                subjBindToAdd.Teacher = existedTeacher;
                            }
                            originalItem.StudentToTeachers.Add(subjBindToAdd);
                            context.Entry(subjBindToAdd).State = subjBindToAdd.Id > 0 ? EntityState.Unchanged : EntityState.Added;
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public void Remove(params Student[] items)
        {
            using (var context = new ArtCollegeContext())
            {
                foreach (var item in items)
                {
                    foreach (var instrument in item.Instruments)
                    {
                        var existedInstrument = context.Instruments.Local.SingleOrDefault(p => p.Id == instrument.Id);
                        if (existedInstrument == null)
                        {
                            context.Entry(instrument).State = EntityState.Unchanged;
                        }
                    }
                    foreach (var bind in item.StudentToTeachers)
                    {
                        context.StudentsToTeachers.Remove(
                            context.StudentsToTeachers.SingleOrDefault(la => la.Id == bind.Id));
                    }
                    item.StudentToTeachers.Clear();
                    context.Entry(item).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
    }
}