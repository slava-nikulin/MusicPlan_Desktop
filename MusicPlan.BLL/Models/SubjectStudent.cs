using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlan.BLL.Models
{
    public class SubjectStudent: IModel
    {
        public virtual Subject Subject { get; set; }
        public virtual Student Student { get; set; }
        public virtual SubjectParameterType SubjectType { get; set; }
        public virtual Instrument Instrument { get; set; }

        public int Id { get; set; }
    }
}
