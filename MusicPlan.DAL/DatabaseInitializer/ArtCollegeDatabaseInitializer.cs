﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan.DAL.DatabaseInitializer
{
    public class ArtCollegeDatabaseInitializer : IDatabaseInitializer<ArtCollegeContext>
    {
        public void InitializeDatabase(ArtCollegeContext context)
        {
            context.Database.CreateIfNotExists();

            context.ParameterTypes.AddRange(new[]
            {
                new SubjectParameterType
                {
                    Name = DalResources.ResourceManager.GetString("StandatdParameters")
                },
                new SubjectParameterType
                {
                    Name = DalResources.ResourceManager.GetString("ConcertMasterParameters")
                },
            });

            context.SaveChanges();
        }
    }
}
