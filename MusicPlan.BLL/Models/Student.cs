using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace MusicPlan.BLL.Models
{
    public sealed class Student: IModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int StudyYear { get; set; }

        public ICollection<Instrument> Instruments { get; set; }
        public ICollection<SubjectStudent> SubjectToStudents { get; set; }

        public Student()
        {
            Instruments = new List<Instrument>();
            SubjectToStudents = new List<SubjectStudent>();
        }

    }
}