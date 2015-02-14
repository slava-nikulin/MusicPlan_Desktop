using System;
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
    public class SubjectScheduleViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Teacher> _selections;

        public ObservableCollection<Teacher> Selections
        {
            get { return _selections; }
            set
            {
                _selections = value;
                OnPropertyChanged();
            }
        }

        public Subject Subject { get; set; }

        public string DisplayName
        {
            get { return string.Format("{0} ({1})", Subject.DisplayName, SubjectParameter.Type.ShortDisplayName); }
        }
        public SubjectParameters SubjectParameter { get; set; }

        public SubjectScheduleViewModel(Subject subject, SubjectParameters parameter)
        {
            Selections = new ObservableCollection<Teacher>();
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
