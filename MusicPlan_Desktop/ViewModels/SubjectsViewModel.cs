using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GeorgeCloney;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;
using MusicPlan_Desktop.CLasses.Events;
using MusicPlan_Desktop.Resources;

namespace MusicPlan_Desktop.ViewModels
{
    public class SubjectsViewModel : BindableBase, ICrudViewModel<Subject>
    {
        #region Private fields
        private Subject _selectedItem;
        private ObservableCollection<Subject> _itemsList;
        private string _btnAddButtonContent;
        private int _selectedItemIndex;
        private IUnityContainer _container;
        private IEventAggregator _eventAggregator;
        #endregion

        #region Constructor
        public SubjectsViewModel(IUnityContainer container)
        {
            _container = container;
            _eventAggregator = _container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<SyncDataEvent>().Subscribe(ReBindItems, true);
            PrepareViewModel();
        }
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

        public Subject SelectedItem
        {
            get { return _selectedItem ?? new Subject(); }
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        public ObservableCollection<Subject> ItemsList
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

        public ICommand AddUpdateCommand{ get; set; }

        public ICommand DeleteItemCommand{ get; set; }

        public ICommand CancelSelectionCommand{ get; set; }

        public ICommand SelectItemCommand{ get; set; }
        #endregion

        #region ViewModel methods
        public void PrepareViewModel()
        {
            BindItems();
            SelectedItem = new Subject();
            SelectedItemIndex = -1;
            AddUpdateCommand = new DelegateCommand<Subject>(AddUpdateItem);
            DeleteItemCommand = new DelegateCommand<Subject>(DeleteItem);
            SelectItemCommand = new DelegateCommand<Subject>(SelectItem);
            CancelSelectionCommand = new DelegateCommand(UnselectItem);
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
        }

        public void UnselectItem()
        {
            SelectedItem = new Subject();
            SelectedItemIndex = -1;
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
        }

        public void SelectItem(Subject item)
        {
            if (SelectedItemIndex != -1)
            {
                SelectedItem = item.DeepClone();
                BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Edit");
            }
        }

        public void BindItems()
        {
            var rep = new ArtCollegeGenericDataRepository<Subject>();
            ItemsList = new ObservableCollection<Subject>((rep.GetAll(la=>la.RegularParameters, la=>la.СoncertmasterParameters)).OrderBy(la => la.Id));
        }

        public void DeleteItem(Subject item)
        {
            var rep = new ArtCollegeGenericDataRepository<Subject>();
            rep.Remove(item);
            BindItems();
            UnselectItem();
        }

        public void AddUpdateItem(Subject item)
        {
            var rep = new ArtCollegeGenericDataRepository<Subject>();
            if (item.Id == 0)
            {
                rep.Add(item);
            }
            else
            {
                rep.Update(item);
            }
            BindItems();
            UnselectItem();
        }

        public void ReBindItems(object obj)
        {
            BindItems();
        }
        #endregion
    }
}
