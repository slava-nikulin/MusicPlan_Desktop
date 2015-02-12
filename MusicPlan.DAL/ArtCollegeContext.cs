using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.DatabaseInitializer;

namespace MusicPlan.DAL
{
    public class ArtCollegeContext : DbContext
    {
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SubjectParameters> SubjectsParameters { get; set; }
        public DbSet<SubjectParameterType> ParameterTypes { get; set; }
        public DbSet<SubjectStudent> SubjectsToStudents { get; set; } 

        public ArtCollegeContext(): base("ArtCollegeDbConnection")
        {
            Database.SetInitializer(new ArtCollegeDatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instrument>().HasKey(i => i.Id);
            modelBuilder.Entity<Student>().HasKey(st => st.Id);
            modelBuilder.Entity<Subject>().HasKey(su => su.Id);
            modelBuilder.Entity<Teacher>().HasKey(t => t.Id);
            modelBuilder.Entity<SubjectParameterType>().HasKey(la => la.Id);
            modelBuilder.Entity<SubjectStudent>().HasKey(sp => sp.Id);
            modelBuilder.Entity<SubjectParameters>().HasKey(sp => sp.Id);

            modelBuilder.Entity<Student>()
                .HasMany(st => st.Instruments)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("InstrumentId");
                });

            modelBuilder.Entity<Student>().HasMany(st => st.SubjectToStudents).WithRequired(su=>su.Student).Map(m =>
            {
                m.MapKey("StudentId");
            });

            modelBuilder.Entity<Subject>().HasMany(su => su.SubjectToStudents).WithRequired(t => t.Subject).Map(m =>
            {
                m.MapKey("SubjectId");
            });

            modelBuilder.Entity<Subject>().HasMany(su => su.Teachers).WithMany(t => t.Subjects).Map(m =>
            {
                m.MapLeftKey("SubjectId");
                m.MapRightKey("TeacherId");
            });

            modelBuilder.Entity<Subject>()
                .HasMany(subj => subj.HoursParameters)
                .WithRequired(param => param.Subject)
                .Map(
                    m =>
                    {
                        m.MapKey("SubjectId");
                    });

            modelBuilder.Entity<SubjectParameters>().HasRequired(la => la.Type).WithMany().Map(m =>
            {
                m.MapKey("TypeId");
            });

            modelBuilder.Entity<SubjectParameters>().Ignore(p => p.DisplayName);
            modelBuilder.Entity<Teacher>().Ignore(t => t.DisplayName);
            modelBuilder.Entity<Instrument>().Ignore(t => t.DisplayName);
            modelBuilder.Entity<Subject>().Ignore(t => t.DisplayName);
            modelBuilder.Entity<Student>().Ignore(t => t.DisplayName);
            modelBuilder.Entity<SubjectParameterType>().Ignore(t => t.ShortDisplayName);
            modelBuilder.Entity<Subject>().Ignore(t => t.HoursParametersSorted);
        }
    }
}
