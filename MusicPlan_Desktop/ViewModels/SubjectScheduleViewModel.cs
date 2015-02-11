using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;
using MusicPlan.BLL.Models;

namespace MusicPlan_Desktop.ViewModels
{
    public class SubjectScheduleViewModel:BindableBase
    {
        private List<Teacher> _selections;

        public List<Teacher> Selections
        {
            get { return _selections; }
            set { SetProperty(ref _selections, value); }
        }

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
