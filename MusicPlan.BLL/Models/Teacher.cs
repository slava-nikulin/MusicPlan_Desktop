﻿using System;
using System.Collections.Generic;
using MusicPlan.BLL.Models;

namespace MusicPlan.BLL.Models
{
    [Serializable]
    public class Teacher : IModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string DisplayName
        {
            get
            {
                return string.Format("{0}. {1}. {2}", FirstName[0], MiddleName[0], LastName);
            }
        }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
