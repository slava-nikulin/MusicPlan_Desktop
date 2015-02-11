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
        public List<SubjectScheduleViewModel> AvailableSubjects { get; set; }
        public Student Student { get; set; }

        public StudentsScheduleViewModel(Student student, IEnumerable<Subject> subjects)
        {
            AvailableSubjects = new List<SubjectScheduleViewModel>();
            Student = student;
            foreach (var subject in subjects)
            {
                foreach (var param in subject.HoursParameters.Where(la=>la.StudyYear==student.StudyYear))
                {
                    AvailableSubjects.Add(new SubjectScheduleViewModel(subject, param));
                }
            }
        }
    }
}
