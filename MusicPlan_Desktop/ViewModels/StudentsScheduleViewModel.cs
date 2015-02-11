using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using MusicPlan.BLL.Models;

namespace MusicPlan_Desktop.ViewModels
{
    public class StudentsScheduleViewModel:BindableBase
    {
        private List<SubjectScheduleViewModel> _availableSubjects;

        public List<SubjectScheduleViewModel> AvailableSubjects
        {
            get { return _availableSubjects; }
            set { SetProperty(ref _availableSubjects, value); }
        }

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
