using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using MusicPlan_Desktop.Resources;

namespace MusicPlan_Desktop.ViewModels
{
    public class SchedulesMainViewModel:BindableBase
    {
        #region Private fields
        private int _selectedYear;
        private bool _isMainGridVisible;
        private string _scheduleHeader;
        private ScheduleByYearViewModel _mainContainerDataContext;
        private IUnityContainer _container;

        #endregion

        #region Public properties
        public string ScheduleHeader
        {
            get { return _scheduleHeader; }
            set { SetProperty(ref _scheduleHeader, value); }
        }

        public bool IsMainGridVisible
        {
            get { return _isMainGridVisible; }
            set { SetProperty(ref _isMainGridVisible, value); }
        }

        public List<int> Classes
        {
            get { return new List<int> {1, 2, 3, 4, 5}; }
        }

        public ScheduleByYearViewModel MainContainerDataContext
        {
            get { return _mainContainerDataContext; }
            set { SetProperty(ref _mainContainerDataContext, value); }
        }

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                IsMainGridVisible = true;
                ScheduleHeader = string.Format(ApplicationResources.ScheduleForSpecificYear, _selectedYear);
                MainContainerDataContext =
                    _container.Resolve<ScheduleByYearViewModel>(new ParameterOverride("studyYear", _selectedYear),
                        new ParameterOverride("container", _container));
                OnPropertyChanged(() => SelectedYear);
            }
        }
        #endregion

        #region Constructor
        public SchedulesMainViewModel(IUnityContainer container)
        {
            _container = container;
            IsMainGridVisible = false;
        }
        #endregion

        #region ViewModel methods
        #endregion

    }
}
