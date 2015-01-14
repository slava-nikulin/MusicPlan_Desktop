using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;

namespace MusicPlan.DAL
{
    public class ArtCollegeContext : DbContext
    {
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SubjectParameters> SubjectsParameters { get; set; } 

        public ArtCollegeContext(): base("ArtCollegeDbConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ArtCollegeContext>());
            var a = ConfigurationManager.ConnectionStrings;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instrument>().HasKey(i => i.Id);
            modelBuilder.Entity<Student>().HasKey(st => st.Id);
            modelBuilder.Entity<Subject>().HasKey(su => su.Id);
            modelBuilder.Entity<Teacher>().HasKey(t => t.Id);
            modelBuilder.Entity<SubjectParameters>().HasKey(sp => sp.Id);

            modelBuilder.Entity<Student>()
                .HasMany(st => st.Instruments)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("InstrumentId");
                });
            modelBuilder.Entity<Student>().HasMany(st => st.Subjects).WithMany(su=>su.Students).Map(m =>
            {
                m.MapLeftKey("StudentId");
                m.MapRightKey("SubjectId");
            });
            modelBuilder.Entity<Subject>().HasMany(su => su.Teachers).WithMany(t => t.Subjects).Map(m =>
            {
                m.MapLeftKey("SubjectId");
                m.MapRightKey("TeacherId");
            });


        }
    }
}
