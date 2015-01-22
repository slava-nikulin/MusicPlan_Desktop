using System.Collections.Generic;

namespace MusicPlan.BLL.Models
{
    public class Subject: IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual SubjectParameters RegularParameters { get; set; }
        public virtual SubjectParameters СoncertmasterParameters { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<SubjectStudent> SubjectToStudents { get; set; }
    }
}
