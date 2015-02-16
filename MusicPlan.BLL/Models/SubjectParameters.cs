using System;

namespace MusicPlan.BLL.Models
{
    [Serializable]
    public class SubjectParameters : IModel
    {
        public long Id { get; set; }
        public int WeeksPerFirstSemester { get; set; }
        public int WeeksPerSecondSemester { get; set; }
        public int HoursPerFirstSemester { get; set; }
        public int HoursPerSecondSemester { get; set; }
        public int? StudyYear { get; set; }
        public string DisplayName
        {
            get
            {
                return string.Format("{0}*{1}+{2}*{3}={4} ({5}.)", WeeksPerFirstSemester,
                    HoursPerFirstSemester, WeeksPerSecondSemester,
                    HoursPerSecondSemester,
                    WeeksPerFirstSemester * HoursPerFirstSemester +
                    WeeksPerSecondSemester * HoursPerSecondSemester, Type.Name.Substring(0,4));
            }
        }

        public virtual Subject Subject { get; set; }
        public virtual SubjectParameterType Type { get; set; }


        public SubjectParameters()
        {
            Type = new SubjectParameterType();
        }
    }
}
