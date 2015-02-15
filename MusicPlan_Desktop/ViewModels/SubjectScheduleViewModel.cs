using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Practices.Prism.Mvvm;
using MusicPlan.BLL.Models;
using MusicPlan_Desktop.Annotations;

namespace MusicPlan_Desktop.ViewModels
{
    [Serializable]
    public class SubjectScheduleViewModel
    {
        private IList _selections = new ObservableCollection<Teacher>();

        public IList Selections
        {
            get { return _selections; }
        }

        public Subject Subject { get; set; }

        public string DisplayName
        {
            get { return string.Format("{0} ({1})", Subject.DisplayName, SubjectParameter.Type.ShortDisplayName); }
        }
        public SubjectParameters SubjectParameter { get; set; }

        public SubjectScheduleViewModel(Subject subject, SubjectParameters parameter)
        {
            SubjectParameter = parameter;
            Subject = subject;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
