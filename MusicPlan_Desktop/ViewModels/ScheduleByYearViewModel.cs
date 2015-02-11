using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;
using MusicPlan_Desktop.CLasses.Events;

namespace MusicPlan_Desktop.ViewModels
{
    public class ScheduleByYearViewModel : BindableBase
    {
        #region Private fields
        private ObservableCollection<StudentsScheduleViewModel> _studentsList;
        private int _selectedStudentIndex;
        private int _studyYear;
        private IEventAggregator _eventAggregator;

        #endregion

        #region Public properties
        public ObservableCollection<StudentsScheduleViewModel> StudentsList
        {
            get { return _studentsList; }
            set { SetProperty(ref _studentsList, value); }
        }

        public int SelectedStudentIndex
        {
            get { return _selectedStudentIndex; }
            set { SetProperty(ref _selectedStudentIndex, value); }
        }
        #endregion

        #region Constructor
        public ScheduleByYearViewModel(int studyYear, IUnityContainer container)
        {
            _studyYear = studyYear;
            RebindItems(_studyYear);
            _eventAggregator = container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<SyncDataEvent>().Subscribe(RebindItems, true);
        }

        private void RebindItems(object obj)
        {
            BindItems(_studyYear);
            SelectedStudentIndex = -1;
        }

        private void BindItems(int studyYear)
        {
            var rep1 = new ArtCollegeGenericDataRepository<Subject>();
            var allSubjects =
                new ObservableCollection<Subject>(
                    rep1.GetList(la => la.HoursParameters.Any(param => param.StudyYear == studyYear), la => la.Teachers,
                        la => la.HoursParameters, la => la.HoursParameters.Select(p => p.Type)));
            var rep = new ArtCollegeGenericDataRepository<Student>();
            var allStudents = new ObservableCollection<Student>(rep.GetList(la => la.StudyYear == studyYear));
            StudentsList = new ObservableCollection<StudentsScheduleViewModel>();
            foreach (var student in allStudents)
            {
                StudentsList.Add(new StudentsScheduleViewModel(student, allSubjects));
            }
        }

        #endregion

        #region ViewModel methods

        #endregion
    }
}
