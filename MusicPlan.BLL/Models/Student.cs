using System;
using System.Collections.Generic;

namespace MusicPlan.BLL.Models
{
    public class Student: IModel, ICloneable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int StudyYear { get; set; }

        public virtual ICollection<Instrument> Instruments { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }

        public Student()
        {
            Instruments = new List<Instrument>();
            Subjects = new List<Subject>();
        }

        public object Clone()
        {
            var obj = (Student)MemberwiseClone();
            obj.Instruments = new List<Instrument>();
            foreach (var instr in Instruments)
            {
                obj.Instruments.Add((Instrument)instr.Clone());
            }
            return obj;
        }
    }
}