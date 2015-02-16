using System;

namespace MusicPlan.BLL.Models
{
    public class Instrument: IModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string DisplayName { get { return Name; } }
    }
}
