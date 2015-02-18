using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using MusicPlan.DAL.Repository;
using MusicPlan_Desktop.CLasses;
using MusicPlan_Desktop.CLasses.Events;
using MusicPlan_Desktop.Resources;

namespace MusicPlan_Desktop.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private IUnityContainer _container;
        private string _statusMessage;

        public ICommand UpgradeStudentsCommand { get; set; }
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }

        public MainViewModel(IUnityContainer container)
        {
            _container = container;
            _eventAggregator = _container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<ShowStatusMessageEvent>().Subscribe(ShowStatus, true);
            UpgradeStudentsCommand = new DelegateCommand(UpgradeStudents);
        }

        private void UpgradeStudents()
        {
            var repo = new StudentRepository();
            repo.UpgradeAllStudents();
            _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
            _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.StudentsUpgraded);
        }

        private void ShowStatus(string status)
        {
            StatusMessage = status;
            Task.Run(() =>
            {
                var origString = status;
                Thread.Sleep(3000);
                if (origString == StatusMessage)
                {
                    StatusMessage = string.Empty;
                }
            });
        }
    }
}
