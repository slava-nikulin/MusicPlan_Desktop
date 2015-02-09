using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using MusicPlan.BLL.ExtendedModels;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;

namespace MusicPlan_Desktop.ViewModels
{
    public class SchedulesViewModel : BindableBase
    {
        #region Private fields
        private ObservableCollection<StudentsScheduleViewModel> _studentsList;
        private int _selectedStudentIndex;
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
        public SchedulesViewModel(int studyYear)
        {
            BindItems(studyYear);
        }

        private void BindItems(int studyYear)
        {
            var rep1 = new ArtCollegeGenericDataRepository<Subject>();
            var allSubjects =
                new ObservableCollection<Subject>(
                    rep1.GetList(la => la.HoursParameters.Any(param => param.StudyYear == studyYear), la => la.Teachers));
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
