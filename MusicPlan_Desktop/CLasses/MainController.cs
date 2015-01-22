using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace MusicPlan_Desktop
{
    public class MainController
    {
        private readonly IUnityContainer _container;
        private IRegionManager regionManager;
        private IEventAggregator eventAggregator = null;

        public MainController(IUnityContainer container)
        {
            this._container = container;
            this.logger = this._container.Resolve<ILoggerFacade>();
            this.eventAggregator = this._container.Resolve<IEventAggregator>();
            this.regionManager = this._container.Resolve<IRegionManager>();

            /// ***  The controller remains active because event subscriptions(see keepSubscriberReferenceAlive) - but that is it's reason for existence. ***
            eventAggregator.GetEvent<ShowCustomerEvent>().(ShowCustomer, true);
        }

    }
}
