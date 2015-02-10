using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using MusicPlan_Desktop.Modules;

namespace MusicPlan_Desktop
{
    public class Bootstrapper : UnityBootstrapper
    {

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var moduleCatalog = new ModuleCatalog();

            var mainModule = typeof(MainModule);
            moduleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = mainModule.Name,
                ModuleType = mainModule.AssemblyQualifiedName
            });
            var instrumentsModule = typeof(InstrumentsModule);
            moduleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = instrumentsModule.Name,
                ModuleType = instrumentsModule.AssemblyQualifiedName
            });
            var studentsModule = typeof(StudentsModule);
            moduleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = studentsModule.Name,
                ModuleType = studentsModule.AssemblyQualifiedName
            });
            var subjectsModule = typeof(SubjectsModule);
            moduleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = subjectsModule.Name,
                ModuleType = subjectsModule.AssemblyQualifiedName
            });
            var teachersModule = typeof(TeachersModule);
            moduleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = teachersModule.Name,
                ModuleType = teachersModule.AssemblyQualifiedName
            });
            var schedulesModule = typeof(SchedulesModule);
            moduleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = schedulesModule.Name,
                ModuleType = schedulesModule.AssemblyQualifiedName
            });

            return moduleCatalog;
        }

        /// <summary>
        /// Configure unity container
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }

        protected override DependencyObject CreateShell()
        {
            return new MainWindow();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }
    }
}
