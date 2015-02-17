using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClosedXML.Excel;
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
    public class PreloadViewModel : BindableBase
    {
        #region Private properties

        private ObservableCollection<Teacher> _teachersList;
        private IEventAggregator _eventAggregator;
        private int _selectedTeacherindex;
        private Teacher _selectedTeacher;
        private ObservableCollection<TeacherHoursPreloadViewModel> _hoursPreload;
        private int _totalHours;

        #endregion


        #region Public properties

        public ObservableCollection<TeacherHoursPreloadViewModel> HoursPreload
        {
            get { return _hoursPreload; }
            set { SetProperty(ref _hoursPreload, value); }
        }

        public Teacher SelectedTeacher
        {
            get { return _selectedTeacher; }
            set
            {
                SetProperty(ref _selectedTeacher, value);
                if (_selectedTeacher != null)
                {
                    PrepareHoursPreload();
                }
            }
        }

        public ObservableCollection<Teacher> TeachersList
        {
            get { return _teachersList; }
            set { SetProperty(ref _teachersList, value); }
        }

        public ICommand ExportToExcelCommand { get; set; }

        public int SelectedTeacherindex
        {
            get { return _selectedTeacherindex; }
            set { SetProperty(ref _selectedTeacherindex, value); }
        }

        public int TotalHours
        {
            get { return _totalHours; }
            set { SetProperty(ref _totalHours, value); }
        }

        #endregion


        #region Constructor

        public PreloadViewModel(IUnityContainer container)
        {
            _eventAggregator = container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<SyncDataEvent>().Subscribe(ReBindItems, true);

            PrepareViewModel();
        }

        #endregion


        #region ViewModel methods

        private void PrepareHoursPreload()
        {
            var rep = new ArtCollegeGenericDataRepository<StudentToTeacher>();
            HoursPreload = new ObservableCollection<TeacherHoursPreloadViewModel>(rep.GetList(
                la => la.Teacher.Id == SelectedTeacher.Id,
                la => la.Student, la => la.Subject, la => la.SubjectType, la => la.Teacher,
                la => la.Subject.HoursParameters, la => la.Subject.HoursParameters.Select(p => p.Type))
                .OrderBy(la => la.SubjectType.Id)
                .ThenBy(la => la.Student.StudyYear)
                .ThenBy(la => la.Student.LastName)
                .Select(la => new TeacherHoursPreloadViewModel
                {
                    StudentName = la.Student.LastName,
                    StudyYear = la.Student.StudyYear,
                    HoursTotal = la.Hours.TotalHours,
                    HoursFormula = la.Hours.DisplayName,
                    SubjectType = la.Hours.Type,
                    SubjectName = la.Subject.Name,
                    Comment = string.Empty,
                    HoursCount1Semester = la.Hours.HoursPerFirstSemester,
                    HoursCount2Semester = la.Hours.HoursPerSecondSemester,
                    WeeksCount1Semester = la.Hours.WeeksPerFirstSemester,
                    WeeksCount2Semester = la.Hours.WeeksPerSecondSemester
                }));

            TotalHours = HoursPreload.Sum(la => la.HoursTotal);
        } 

        private void PrepareViewModel()
        {
            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            ReBindItems(null);
        }

        private void ExportToExcel()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var di = Directory.CreateDirectory(path);
            di.CreateSubdirectory(ApplicationResources.ApplicationDocumentsFolder);
            var appPath = di.GetDirectories(ApplicationResources.ApplicationDocumentsFolder)[0];
            var years = DateTime.Now.Month < 6
                ? string.Format("{0}-{1}", DateTime.Now.Year - 1, DateTime.Now.Year)
                : string.Format("{0}-{1}", DateTime.Now.Year, DateTime.Now.Year + 1);
            var fileName = string.Format("{0}_{1}", SelectedTeacher.DisplayName, years);
            var filePath = Path.Combine(appPath.FullName, string.Format("{0}.xlsx", fileName));

            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(fileName);

            ws.Cell("A1").Value = "Педагогическая нагрузка";
            ws.Cell("A1").Style.Font.FontSize = 18;
            ws.Cell("A1").Style.Font.SetBold();
            ws.Range("A1:J1").Merge();
            ws.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("A2").Value = "\"Музыкальное искусство эстрады\"";
            ws.Cell("A2").Style.Font.FontSize = 15;
            ws.Cell("A2").Style.Font.SetBold();
            ws.Cell("A2").Style.Font.SetItalic();
            ws.Range("A2:J2").Merge();
            ws.Cell("A2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("A3").Value = "дневное отделение";
            ws.Range("A3:J3").Merge();
            ws.Cell("A3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            var title = string.Format("Преднагрузка. Преподаватель {0}", SelectedTeacher.DisplayName);
            ws.Cell("A4").Value = title;
            ws.Range("A4:J4").Merge();
            ws.Cell("A4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Cell("A4")
                .RichText.Substring(title.Length - 1 - SelectedTeacher.DisplayName.Length)
                .SetItalic()
                .SetBold()
                .SetFontSize(15);

            ws.Cell("A5").Value = string.Format("На {0} учебный год", years);
            ws.Range("A5:J5").Merge();
            ws.Cell("A5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //ws.Column("A").AdjustToContents();

            var table = ws.Range("A7:J8");

            table.Cell("A1").Value = "№";
            table.Cell("B1").Value = "Фамилия";
            table.Cell("C1").Value = "Предмет";
            table.Cell("D1").Value = "Курс";
            table.Cell("E1").Value = "Кол-во недель";
            table.Cell("G1").Value = "Кол-во часов";
            table.Cell("I1").Value = "Годовых";
            table.Cell("J1").Value = "Примечание";
            table.Cell("E2").Value = "1 семестр";
            table.Cell("F2").Value = "2 семестр";
            table.Cell("G2").Value = "1 семестр";
            table.Cell("H2").Value = "2 семестр";

            table.Range("A1:A2").Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            table.Range("B1:B2").Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            table.Range("C1:C2").Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            table.Range("D1:D2").Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            table.Range("E1:F1").Merge();
            table.Range("G1:H1").Merge();
            table.Range("I1:I2").Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            table.Range("J1:J2").Merge().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            table.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            table.Style.Border.InsideBorder = XLBorderStyleValues.Medium;

            var groups = HoursPreload.GroupBy(la => la.SubjectType).ToArray();
            var tableBody = ws.Range("A9:J" + (9 + HoursPreload.Count + groups.Length));
            tableBody.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

            //TODO:
            //var index = 1;
            //foreach (var group in groups)
            //{
            //    tableBody.Cell("A"+index).Value = group.Key.Name;
            //    tableBody.Range(string.Format("A{0}:J{0}", index)).Merge();
            //    foreach (var row in group)
            //    {
            //        tableBody.Cell("A"+index+1).Value = 
            //    }
                
            //}


            wb.SaveAs(filePath);
        }

        private void ReBindItems(object obj)
        {
            var teacherRep = new ArtCollegeGenericDataRepository<Teacher>();
            TeachersList =
                new ObservableCollection<Teacher>(teacherRep.GetAll(la => la.Subjects).OrderBy(la => la.LastName));
            SelectedTeacherindex = -1;
            SelectedTeacher = null;
            HoursPreload = null;
        }

        #endregion

    }
}
