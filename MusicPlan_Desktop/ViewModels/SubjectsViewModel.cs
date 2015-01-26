using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Proxies;
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
        private SubjectParameters _selectedSubItem;
        private ObservableCollection<Subject> _itemsList;
        private ObservableCollection<SubjectParameters> _subItemsList;
        private ObservableCollection<SubjectParameterType> _subjectParameterTypes;
        private string _btnAddButtonContent;
        private int _selectedItemIndex;
        private int _selectedSubItemIndex;
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

        public List<int> Classes
        {
            get { return new List<int> { 1, 2, 3, 4, 5 }; }
        }

        public ObservableCollection<SubjectParameterType> SubjectParameterTypes
        {
            get { return _subjectParameterTypes; }
            set { SetProperty(ref _subjectParameterTypes, value); }
        }

        public SubjectParameters SelectedSubItem
        {
            get { return _selectedSubItem; }
            set
            {
                SetProperty(ref _selectedSubItem, value);
            } 
        }

        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set
            {
                SetProperty(ref _selectedItemIndex, value);
            }
        }

        public int SelectedSubItemIndex
        {
            get { return _selectedSubItemIndex; }
            set
            {
                SetProperty(ref _selectedSubItemIndex, value);
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

        public ObservableCollection<SubjectParameters> SubItemsList
        {
            get { return _subItemsList; }
            set
            {
                SetProperty(ref _subItemsList, value);
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
        public ICommand DeleteSubItemCommand { get; set; }
        public ICommand SelectSubItemCommand { get; set; }
        #endregion

        #region ViewModel methods
        public void PrepareViewModel()
        {
            BindItems();
            SelectedItem = new Subject();
            SelectedSubItem = new SubjectParameters();
            SelectedItemIndex = -1;
            SelectedSubItemIndex = -1;
            AddUpdateCommand = new DelegateCommand<Subject>(AddUpdateItem);
            DeleteItemCommand = new DelegateCommand<Subject>(DeleteItem);
            DeleteSubItemCommand = new DelegateCommand<SubjectParameters>(DeleteSubItem);
            SelectItemCommand = new DelegateCommand<Subject>(SelectItem);
            SelectItemCommand = new DelegateCommand<SubjectParameters>(SelectSubItem);
            CancelSelectionCommand = new DelegateCommand(UnselectItem);
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
        }

        private void SelectSubItem(SubjectParameters subItem)
        {
            if (SelectedSubItemIndex != -1)
            {
                SelectedSubItem = subItem.DeepClone();
            }
        }

        private void DeleteSubItem(SubjectParameters subItem)
        {
            var rep = new ArtCollegeGenericDataRepository<SubjectParameters>();
            rep.Remove(subItem);
            UnselectSubItem();
            BindSubItems();
        }

        public void UnselectSubItem()
        {
            SelectedSubItem = new SubjectParameters();
            SelectedItemIndex = -1;
        }

        public void UnselectItem()
        {
            UnselectSubItem();
            SelectedItem = new Subject();
            SelectedSubItemIndex = -1;
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

        public void BindSubItems()
        {
            var rep = new ArtCollegeGenericDataRepository<SubjectParameters>();
            SubItemsList =
                new ObservableCollection<SubjectParameters>(
                    (rep.GetAll(la => la.Type).OrderBy(la => la.Id)));
        }

        public void BindItems()
        {
            var rep = new ArtCollegeGenericDataRepository<Subject>();
            ItemsList =
                new ObservableCollection<Subject>(
                    (rep.GetAll(la => la.HoursParameters, la => la.HoursParameters.Select(p => p.Type))).OrderBy(la => la.Id));
            var rep1 = new ArtCollegeGenericDataRepository<SubjectParameterType>();
            SubjectParameterTypes = new ObservableCollection<SubjectParameterType>(rep1.GetAll());
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
                //rep.Add(item);
            }
            else
            {
                //rep.Update(item);
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
