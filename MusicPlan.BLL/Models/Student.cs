using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace MusicPlan.BLL.Models
{
    public class Student: IModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int StudyYear { get; set; }

        public virtual ICollection<Instrument> Instruments { get; set; }
        public virtual ICollection<StudentToTeacher> StudentToTeachers { get; set; }

        public string DisplayName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public Student()
        {
            Instruments = new List<Instrument>();
            StudentToTeachers = new List<StudentToTeacher>();
        }

    }
}