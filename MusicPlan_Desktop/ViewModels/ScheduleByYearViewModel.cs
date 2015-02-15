using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;
using MusicPlan_Desktop.CLasses.Events;
using MusicPlan_Desktop.Resources;

namespace MusicPlan_Desktop.ViewModels
{
    public class ScheduleByYearViewModel : BindableBase
    {
        #region Private fields

        private int _selectedStudentIndex;
        private readonly int _studyYear;
        private readonly IEventAggregator _eventAggregator;
        private DataTable _mainDt;

        #endregion

        #region Public properties
        public ICommand SaveCommand { get; set; }
        public ICommand DataGridLoaded { get; set; }

        public DataTable MainDt
        {
            get { return _mainDt; }
            set { SetProperty(ref _mainDt, value); }
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
            SaveCommand = new DelegateCommand(SaveSchedule);
            DataGridLoaded = new DelegateCommand(LoadSelections);
        }

        private void LoadSelections()
        {
            var rep3 = new ArtCollegeGenericDataRepository<StudentToTeacher>();
            foreach (DataRow row in MainDt.Rows)
            {
                var stud = (StudentScheduleViewModel)row[0];
                for (var i = 1; i < MainDt.Columns.Count; i++)
                {
                    var subj = ((SubjectScheduleViewModel)row[i]);
                    var bindedTeachers =
                        rep3.GetList(
                            la =>
                                subj.SubjectParameter.Type.Id == la.SubjectType.Id
                                && la.Student.Id == stud.Student.Id
                                && la.Subject.Id == subj.Subject.Id
                                && la.Instrument.Id == stud.Instrument.Id,
                            la => la.Student, la => la.Subject, la => la.SubjectType, la => la.Teacher, la => la.Instrument);
                    foreach (var t in bindedTeachers)
                    {
                        subj.Selections.Add(subj.Subject.Teachers.Single(la=>la.Id == t.Teacher.Id));
                    }
                }
            }
        }

        private void SaveSchedule()
        {
            var studRepo = new StudentRepository();
            foreach (DataRow row in MainDt.Rows)
            {
                var studentScheduleViewModel = row[ApplicationResources.Student] as StudentScheduleViewModel;
                var student =
                    studRepo.GetSingle(
                        la => studentScheduleViewModel != null && la.Id == studentScheduleViewModel.Student.Id,
                        la => la.Instruments, la => la.StudentToSubject,
                        la => la.StudentToSubject.Select(la1 => la1.Instrument),
                        la => la.StudentToSubject.Select(la1 => la1.Subject),
                        la => la.StudentToSubject.Select(la1 => la1.Teacher),
                        la => la.StudentToSubject.Select(la1 => la1.SubjectType));
                student.StudentToSubject.Clear();
                for (var i = 1; i < MainDt.Columns.Count; i++)
                {
                    var subjectScheduleViewModule = row[i] as SubjectScheduleViewModel;
                    foreach (Teacher selectedTeacher in subjectScheduleViewModule.Selections)
                    {
                        student.StudentToSubject.Add(new StudentToTeacher
                        {
                            Instrument = studentScheduleViewModel.Instrument,
                            Subject = subjectScheduleViewModule.Subject,
                            SubjectType = subjectScheduleViewModule.SubjectParameter.Type,
                            Teacher = selectedTeacher
                        });
                    }
                }
                studRepo.Update(student);
            }
        }

        private void RebindItems(object obj)
        {
            BindItems(_studyYear);
            LoadSelections();
            SelectedStudentIndex = -1;
        }

        private void BindItems(int studyYear)
        {
            var rep1 = new ArtCollegeGenericDataRepository<Subject>();
            var availableSubjects =
                new ObservableCollection<Subject>(
                    rep1.GetList(la => la.HoursParameters.Any(param => param.StudyYear == studyYear), la => la.Teachers,
                        la => la.HoursParameters, la => la.HoursParameters.Select(p => p.Type)));
            var availableSubjectViewModels = new ObservableCollection<SubjectScheduleViewModel>();
            foreach (var subject in availableSubjects)
            {
                foreach (var param in subject.HoursParameters.Where(la => la.StudyYear == studyYear))
                {
                    availableSubjectViewModels.Add(new SubjectScheduleViewModel(subject, param));
                }
            }
            var rep2 = new ArtCollegeGenericDataRepository<Student>();
            var allStudentsByStudyYear = new ObservableCollection<Student>(rep2.GetList(la => la.StudyYear == studyYear, la => la.Instruments));
            var studentsViewModelList = new ObservableCollection<StudentScheduleViewModel>();
            foreach (var student in allStudentsByStudyYear)
            {
                foreach (var instr in student.Instruments)
                {
                    studentsViewModelList.Add(new StudentScheduleViewModel(student, availableSubjectViewModels, instr));
                }
            }

            var dt = new DataTable();
            dt.Columns.AddRange(new[]
            {
                new DataColumn
                {
                    ColumnName = ApplicationResources.Student,
                    DataType = typeof (StudentScheduleViewModel)
                }
            });

            foreach (var subj in availableSubjectViewModels)
            {
                dt.Columns.Add(new DataColumn
                {
                    DataType = typeof(SubjectScheduleViewModel),
                    ColumnName = subj.DisplayName
                });
            }

            //var rep3 = new ArtCollegeGenericDataRepository<StudentToTeacher>();
            foreach (var stud in studentsViewModelList)
            {
                var newRow = dt.NewRow();
                newRow[ApplicationResources.Student] = stud;
                foreach (var subj in stud.AvailableSubjects)
                {
                    //var bindedTeachers = new ObservableCollection<Teacher>(
                    //    rep3.GetList(
                    //        la =>
                    //            subj.SubjectParameter.Type.Id == la.SubjectType.Id
                    //            && la.Student.Id == stud.Student.Id
                    //            && la.Subject.Id == subj.Subject.Id,
                    //            la => la.Student, la => la.Subject, la => la.SubjectType, la => la.Teacher)
                    //        .Select(la => la.Teacher));
                    //foreach (var t in bindedTeachers)
                    //{
                    //    subj.Selections.Add(t);
                    //}
                    newRow[subj.DisplayName] = subj;
                }
                dt.Rows.Add(newRow);
            }
            dt.DefaultView.Sort = string.Format("{0} {1}", dt.Columns[0].ColumnName, "ASC"); 
            MainDt = dt;
        }

        #endregion

        #region ViewModel methods

        #endregion
    }
}
