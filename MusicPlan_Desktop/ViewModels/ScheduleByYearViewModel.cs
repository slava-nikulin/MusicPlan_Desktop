using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        private ObservableCollection<StudentScheduleViewModel> _studentsList;
        private int _selectedStudentIndex;
        private int _studyYear;
        private ObservableCollection<SubjectScheduleViewModel> _availableSubjects;
        private IEventAggregator _eventAggregator;

        #endregion

        #region Public properties


        public ObservableCollection<SubjectScheduleViewModel> AvailableSubjects
        {
            get { return _availableSubjects; }
            set { SetProperty(ref _availableSubjects, value); }
        }

        public DataTable MainDt { get; set; }

        public ObservableCollection<StudentScheduleViewModel> StudentsList
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
            AvailableSubjects = new ObservableCollection<SubjectScheduleViewModel>();
            foreach (var subject in allSubjects)
            {
                foreach (var param in subject.HoursParameters.Where(la => la.StudyYear == studyYear))
                {
                    AvailableSubjects.Add(new SubjectScheduleViewModel(subject, param));
                }
            }
            var rep = new ArtCollegeGenericDataRepository<Student>();
            var allStudents = new ObservableCollection<Student>(rep.GetList(la => la.StudyYear == studyYear));
            StudentsList = new ObservableCollection<StudentScheduleViewModel>();
            foreach (var student in allStudents)
            {
                foreach (var instr in student.Instruments)
                {
                    StudentsList.Add(new StudentScheduleViewModel(student, allSubjects, instr));
                }
            }

            var dt = new DataTable();
            dt.Columns.AddRange(new[]
            {
                new DataColumn
                {
                    ColumnName = "stud",
                    DataType = typeof (string)
                },
                new DataColumn
                {
                    ColumnName = "instr",
                    DataType = typeof (string)
                }
            });

            foreach (var subj in StudentsList.First().AvailableSubjects)
            {
                dt.Columns.Add(new DataColumn
                {
                    DataType = typeof (SubjectScheduleViewModel),
                    ColumnName = subj.Subject.DisplayName
                });
            }

            foreach (var stud in StudentsList)
            {
                var newRow = dt.NewRow();
                newRow["stud"] = stud.Student.DisplayName;
                newRow["instr"] = stud.Instrument.Name;
                foreach (var subj in stud.AvailableSubjects)
                {
                    newRow[subj.Subject.DisplayName] = subj;
                }
                dt.Rows.Add(newRow);
            }

            MainDt = dt;
        }

        #endregion

        #region ViewModel methods

        #endregion
    }
}
