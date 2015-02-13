using System.Collections.Generic;
using System.Linq;

namespace MusicPlan.BLL.Models
{
    public class Subject: IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubjectParameters> HoursParameters { get; set; }
        public ICollection<SubjectParameters> HoursParametersSorted { get
        {
            return HoursParameters.OrderBy(la => la.StudyYear).ToList();
        } }
        public virtual ICollection<Teacher> Teachers { get; set; }

        public string DisplayName { get { return Name; } }

        public Subject()
        {
            HoursParameters = new List<SubjectParameters>();
            Teachers = new List<Teacher>();
        }
    }
}
