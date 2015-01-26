namespace MusicPlan.BLL.Models
{
    public class SubjectParameters: IModel
    {
        public int Id { get; set; }
        public int WeeksPerFirstSemester { get; set; }
        public int WeeksPerSecondSemester { get; set; }
        public int HoursPerFirstSemester { get; set; }
        public int HoursPerSecondSemester { get; set; }
        public int StudyYear { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual SubjectParameterType Type { get; set; }

        public SubjectParameters()
        {
            Type = new SubjectParameterType();
        }
    }
}
