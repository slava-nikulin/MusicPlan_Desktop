using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan.BLL.ExtendedModels
{
    public class SubjectsScheduleViewModel
    {
        public List<Teacher> SelectedTeachers { get; set; }
        public Subject Subject { get; set; }
        public Dictionary<int, string> AvailableSubjectParameters { get; set; } 

        public SubjectsScheduleViewModel(Subject subject, int studyYear)
        {
            SelectedTeachers = new List<Teacher>();
            AvailableSubjectParameters = new Dictionary<int, string>();
            Subject = subject;
            foreach (var parameter in subject.HoursParameters)
            {
                if (parameter.StudyYear == studyYear)
                {
                    AvailableSubjectParameters.Add(parameter.Id,
                        string.Format("{0}*{1}+{2}*{3}={4}", parameter.WeeksPerFirstSemester,
                            parameter.HoursPerFirstSemester, parameter.WeeksPerSecondSemester,
                            parameter.HoursPerSecondSemester,
                            parameter.WeeksPerFirstSemester*parameter.HoursPerFirstSemester +
                            parameter.WeeksPerSecondSemester*parameter.HoursPerSecondSemester));
                }
                
            }
        }
    }
}
