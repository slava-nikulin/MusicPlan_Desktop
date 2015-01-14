using System.Collections.Generic;

namespace MusicPlan.BLL.Models
{
    public class Student: IModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int StudyYear { get; set; }

        public virtual ICollection<Instrument> Instruments { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}