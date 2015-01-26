using System.Collections.Generic;

namespace MusicPlan.BLL.Models
{
    public class Subject: IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubjectParameters> HoursParameters { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<SubjectStudent> SubjectToStudents { get; set; }

        public Subject()
        {
            HoursParameters = new List<SubjectParameters>();
            Teachers = new List<Teacher>();
            SubjectToStudents = new List<SubjectStudent>();
        }
    }
}
