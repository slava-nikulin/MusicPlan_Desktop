using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GeorgeCloney;
using Microsoft.Practices.Prism.Mvvm;
using MusicPlan.BLL.Models;

namespace MusicPlan_Desktop.ViewModels
{
    public class StudentScheduleViewModel:BindableBase
    {
        private ObservableCollection<SubjectScheduleViewModel> _availableSubjects;

        public ObservableCollection<SubjectScheduleViewModel> AvailableSubjects
        {
            get { return _availableSubjects; }
            set { SetProperty(ref _availableSubjects, value); }
        }

        public Student Student { get; set; }
        public Instrument Instrument { get; set; }

        public StudentScheduleViewModel(Student student, ObservableCollection<SubjectScheduleViewModel> subjects, Instrument instrument)
        {
            AvailableSubjects = subjects.DeepClone();
            Student = student;
            Instrument = instrument;
        }
    }
}
