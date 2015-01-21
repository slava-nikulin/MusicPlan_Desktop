using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using MusicPlan_Desktop.Views;

namespace MusicPlan_Desktop.Modules
{
    public class StudentsModule : IModule
    {
        private readonly IUnityContainer _container;

        public StudentsModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            var regionManager = _container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Constants.StudentsRegion, typeof(StudentsTabView));
        }
    }
}
