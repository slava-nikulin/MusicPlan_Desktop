using System;

namespace MusicPlan.BLL.Models
{
    public class Instrument: IModel, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public Instrument()
        {
            
        }

        //public Instrument(Instrument instrumentToClone)
        //{
        //    Id = instrumentToClone.Id;
        //    Name = instrumentToClone.Name;
        //}
    }
}
