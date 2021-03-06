﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlan.BLL.Models
{
    public class StudentToTeacher: IModel
    {
        public virtual Teacher Teacher { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Student Student { get; set; }
        public virtual SubjectParameterType SubjectType { get; set; }
        public virtual Instrument Instrument { get; set; }

        public SubjectParameters Hours
        {
            get
            {
                return
                    Subject.HoursParameters.SingleOrDefault(
                        la => la.StudyYear == Student.StudyYear && la.Type.Id == SubjectType.Id);
            }
        }

        public long Id { get; set; }
    }
}
