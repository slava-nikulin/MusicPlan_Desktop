using System;

namespace MusicPlan.BLL.Models
{
    [Serializable]
    public class SubjectParameterType : IModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortDisplayName { get { return string.Format("{0}.", Name.Substring(0, 4)); } }
    }
}