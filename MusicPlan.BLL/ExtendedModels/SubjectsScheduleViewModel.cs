using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan.BLL.ExtendedModels
{
    public class SubjectsScheduleViewModel
    {
        public List<Teacher> SelectedTeachers { get; set; }
        public Subject Subject { get; set; }

        public SubjectsScheduleViewModel(Subject subject)
        {
            SelectedTeachers = new List<Teacher>();
            Subject = subject;
        }
    }
}
