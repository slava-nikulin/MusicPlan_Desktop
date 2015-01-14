using System.Collections.Generic;

namespace MusicPlan.BLL.Models
{
    public class Teacher: IModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
