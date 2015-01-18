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

            var instrumentsModule = typeof(InstrumentsModule);
            moduleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = instrumentsModule.Name,
                ModuleType = instrumentsModule.AssemblyQualifiedName
            });

            
            //Type customerModule = typeof(CustomersModule);
            //moduleCatalog.AddModule(new ModuleInfo() { ModuleName = customerModule.Name, ModuleType = customerModule.AssemblyQualifiedName });

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
