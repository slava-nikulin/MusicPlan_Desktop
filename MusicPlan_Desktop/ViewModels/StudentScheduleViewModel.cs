using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using MusicPlan.BLL.Models;

namespace MusicPlan_Desktop.ViewModels
{
    public class StudentScheduleViewModel:BindableBase
    {
        private List<SubjectScheduleViewModel> _availableSubjects;

        public List<SubjectScheduleViewModel> AvailableSubjects
        {
            get { return _availableSubjects; }
            set { SetProperty(ref _availableSubjects, value); }
        }

        public Student Student { get; set; }
        public Instrument Instrument { get; set; }

        public StudentScheduleViewModel(Student student, IEnumerable<Subject> subjects, Instrument instrument)
        {
            AvailableSubjects = new List<SubjectScheduleViewModel>();
            Student = student;
            Instrument = instrument;
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
