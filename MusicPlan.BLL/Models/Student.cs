using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace MusicPlan.BLL.Models
{
    public sealed class Student: IModel, ICloneable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int StudyYear { get; set; }

        public ICollection<Instrument> Instruments { get; set; }
        public ICollection<Subject> Subjects { get; set; }

        public Student()
        {
            Instruments = new List<Instrument>();
            Subjects = new List<Subject>();
        }

        //public Student(Student studForCopy)
        //{
        //    FirstName = studForCopy.FirstName;
        //    LastName = studForCopy.LastName;
        //    MiddleName = studForCopy.MiddleName;
        //    StudyYear = studForCopy.StudyYear;
        //    Id = studForCopy.Id;
        //    Instruments = new List<Instrument>();

        //    foreach (var instr in studForCopy.Instruments)
        //    {
        //        Instruments.Add(new Instrument(instr));
        //    }
        //}

        public object Clone()
        {
            //var obj = (Student)MemberwiseClone();
            //obj.Instruments = new List<Instrument>();
            //foreach (var instr in Instruments)
            //{
            //    obj.Instruments.Add((Instrument)instr.Clone());
            //}
            //return obj;

            DataContractSerializer dcSer = new DataContractSerializer(this.GetType());
            MemoryStream memoryStream = new MemoryStream();

            dcSer.WriteObject(memoryStream, this);
            memoryStream.Position = 0;

            Student newObject = (Student)dcSer.ReadObject(memoryStream);
            return newObject;
        }
    }
}