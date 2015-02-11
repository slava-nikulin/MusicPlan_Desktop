using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan.BLL.ExtendedModels
{
    public class SubjectScheduleViewModel
    {
        public List<Teacher> Selections { get; set; }
        public Subject Subject { get; set; }
        public SubjectParameters SubjectParameter { get; set; }

        public SubjectScheduleViewModel(Subject subject, SubjectParameters parameter)
        {
            Selections = new List<Teacher>();
            SubjectParameter = parameter;
            Subject = subject;
        }
    }
}
