using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan.BLL.ExtendedModels
{
    public class StudentsScheduleViewModel
    {
        public List<SubjectsScheduleViewModel> AvailableSubjects { get; set; }
        public Student Student { get; set; }

        public StudentsScheduleViewModel(Student student, IEnumerable<Subject> subjects)
        {
            AvailableSubjects = new List<SubjectsScheduleViewModel>();
            Student = student;
            foreach (var subject in subjects)
            {
                AvailableSubjects.Add(new SubjectsScheduleViewModel(subject));
            }
        }
    }

    
}
