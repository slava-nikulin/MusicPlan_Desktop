using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan_Desktop.ViewModels
{
    public class TeacherHoursPreloadViewModel
    {
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public int StudyYear { get; set; }
        public int WeeksCount1Semester { get; set; }
        public int WeeksCount2Semester { get; set; }
        public int HoursCount1Semester { get; set; }
        public int HoursCount2Semester { get; set; }
        public int HoursTotal { get; set; }
        public string Comment { get; set; }
        public SubjectParameterType SubjectType { get; set; }
        public string HoursFormula { get; set; }
    }
}
