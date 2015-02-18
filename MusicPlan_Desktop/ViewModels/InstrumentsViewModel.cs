using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GeorgeCloney;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;
using MusicPlan_Desktop.CLasses.Events;
using MusicPlan_Desktop.Modules;
using MusicPlan_Desktop.Resources;

namespace MusicPlan_Desktop.ViewModels
{
    public class InstrumentsViewModel : BindableBase, ICrudViewModel<Instrument>
    {
        #region Private fields
        private Instrument _selectedItem;
        private ObservableCollection<Instrument> _itemsList;
        private string _btnAddButtonContent;
        private int _selectedItemIndex;
        private IUnityContainer _container;
        private IEventAggregator _eventAggregator;
        #endregion

        #region Public properties
        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set
            {
                _selectedItemIndex = value;
                OnPropertyChanged(() => SelectedItemIndex);
            }
        }

        public Instrument SelectedItem
        {
            get { return _selectedItem ?? new Instrument(); }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }
        public ObservableCollection<Instrument> ItemsList
        {
            get { return _itemsList; }
            set
            {
                SetProperty(ref _itemsList, value);
            }
        }
        public string BtnAddButtonContent
        {
            get { return _btnAddButtonContent; }
            set { SetProperty(ref _btnAddButtonContent, value); }
        }

        public ICommand AddUpdateCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand CancelSelectionCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }
        #endregion

        #region Constructor
        public InstrumentsViewModel(IUnityContainer container)
        {
            _container = container;
            _eventAggregator = _container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<SyncDataEvent>().Subscribe(ReBindItems, true);
            PrepareViewModel();
        }
        #endregion

        #region ViewModel methods

        public void PrepareViewModel()
        {
            BindItems();
            SelectedItem = new Instrument();
            SelectedItemIndex = -1;
            AddUpdateCommand = new DelegateCommand<Instrument>(AddUpdateItem);
            DeleteItemCommand = new DelegateCommand<Instrument>(DeleteItem);
            SelectItemCommand = new DelegateCommand<Instrument>(SelectItem);
            CancelSelectionCommand = new DelegateCommand(UnselectItem);
            BtnAddButtonContent = ApplicationResources.Insert;

        }

        public void UnselectItem()
        {
            SelectedItem = new Instrument();
            SelectedItemIndex = -1;
            BtnAddButtonContent = ApplicationResources.Insert;
        }

        public void SelectItem(Instrument item)
        {
            if (SelectedItemIndex != -1 && item.Id != SelectedItem.Id)
            {
                SelectedItem = item.DeepClone();
                BtnAddButtonContent = ApplicationResources.Edit;
            }
        }

        public void ReBindItems(object obj)
        {
            BindItems();
        }

        public void BindItems()
        {
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            ItemsList = new ObservableCollection<Instrument>((rep.GetAll()).OrderBy(la => la.Name));
        }

        public void DeleteItem(Instrument item)
        {
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            rep.Remove(item);
            BindItems();
            UnselectItem();
            _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
            _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.InstrumentDeleted);
        }

        public void AddUpdateItem(Instrument item)
        {
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            if (item.Id == 0)
            {
                rep.Add(item);
                _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.InstrumentAdded);
            }
            else
            {
                 rep.Update(item);
                _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.InstrumentEdited);
            }
            BindItems();
            UnselectItem();
            _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
        }
        #endregion
    }
}