using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using MusicPlan_Desktop.CLasses;
using MusicPlan_Desktop.CLasses.Events;

namespace MusicPlan_Desktop.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private IUnityContainer _container;

        //public ICommand SyncDataCommand { get; set; }

        public MainViewModel(IUnityContainer container)
        {
            _container = container;
            _eventAggregator = _container.Resolve<IEventAggregator>();
            //SyncDataCommand = new DelegateCommand(SyncData);
        }

        //private void SyncData()
        //{
        //    _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
        //}
    }
}
