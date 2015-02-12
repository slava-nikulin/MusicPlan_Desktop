namespace MusicPlan.BLL.Models
{
    public class SubjectParameterType : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDisplayName { get { return string.Format("{0}.", Name.Substring(0, 4)); } }
    }
}