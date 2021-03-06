﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GeorgeCloney;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
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
        private object _itemToDelete;
        private Subject _selectedItem;
        private SubjectParameters _selectedSubItem;
        private ObservableCollection<Subject> _itemsList;
        private ObservableCollection<SubjectParameters> _subItemsList;
        private ObservableCollection<SubjectParameterType> _subjectParameterTypes;
        private string _btnAddButtonContent;
        private string _btnDeleteContent;
        private int _selectedItemIndex;
        private int _selectedSubItemIndex;
        private bool _applyForAllStudyYears;
        private bool _subItemsInsertUpdateMode;
        private List<int> _classes;
        private IUnityContainer _container;
        private IEventAggregator _eventAggregator;
        private bool _subitemClicked;

        #endregion

        #region Public properties

        public object ItemToDelete
        {
            get { return _itemToDelete ?? new { Id = (long)0 }; }
            set { SetProperty(ref _itemToDelete, value); }
        }
        public bool SubItemsInsertUpdateMode
        {
            get { return _subItemsInsertUpdateMode; }
            set
            {
                SetProperty(ref _subItemsInsertUpdateMode, value);

            }
        }

        public bool ApplyForAllStudyYears
        {
            get { return _applyForAllStudyYears; }
            set { SetProperty(ref _applyForAllStudyYears, value); }
        }

        public List<int> Classes
        {
            get { return _classes; }
            set { SetProperty(ref _classes, value); }
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

        public string BtnDeleteContent
        {
            get { return _btnDeleteContent; }
            set { SetProperty(ref _btnDeleteContent, value); }
        }

        public ICommand AddUpdateCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand CancelSelectionCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }
        //public ICommand DeleteSubItemCommand { get; set; }
        public ICommand SelectSubItemCommand { get; set; }
        public ICommand ClickItemCommand { get; set; }
        public ICommand ClickSubItemCommand { get; set; }
        public InteractionRequest<INotification> ErrorPopupViewRequest { get; private set; }
        #endregion

        #region Constructor
        public SubjectsViewModel(IUnityContainer container)
        {
            _subitemClicked = false;
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
            SubItemsInsertUpdateMode = true;
            SelectedItem = new Subject();
            SelectedSubItem = new SubjectParameters();
            Classes = new List<int> { 1, 2, 3, 4, 5 };
            SelectedItemIndex = -1;
            SelectedSubItemIndex = -1;
            AddUpdateCommand = new DelegateCommand<Subject>(AddUpdateItem);
            DeleteItemCommand = new DelegateCommand<object>(DeleteItemOrSubItem);
            //DeleteSubItemCommand = new DelegateCommand<SubjectParameters>(DeleteSubItem);
            SelectItemCommand = new DelegateCommand<Subject>(SelectItem);
            SelectSubItemCommand = new DelegateCommand<SubjectParameters>(SelectSubItem);
            CancelSelectionCommand = new DelegateCommand(UnselectItem);
            ClickItemCommand = new DelegateCommand<Subject>(ClickItem);
            ClickSubItemCommand = new DelegateCommand<SubjectParameters>(ClickSubItem);
            BtnAddButtonContent = ApplicationResources.SubjectInsert_ParameterInsert;
            ErrorPopupViewRequest = new InteractionRequest<INotification>();
            PropertyChanged += ChangeProperty;
        }

        private void ClickItem(Subject item)
        {
            if (item != null && item.Id == SelectedItem.Id && !_subitemClicked)
            {
                UnselectItem();
            }
            else if (!_subitemClicked)
            {
                UnselectSubItem();
                SelectedItem = item.DeepClone();
                BtnAddButtonContent = SubItemsInsertUpdateMode
                    ? ApplicationResources.SubjectEdit_ParameterInsert
                    : ApplicationResources.SubjectEdit;
            }
            _subitemClicked = false;
        }

        private void ClickSubItem(SubjectParameters subItem)
        {
            _subitemClicked = true;
            if (subItem!= null && subItem.Id == SelectedSubItem.Id)
            {
                UnselectSubItem();
            }
            else if (subItem != null)
            {
                ApplyForAllStudyYears = false;
                SubItemsInsertUpdateMode = true;
                SelectedSubItem = subItem.DeepClone();
                BtnAddButtonContent = ApplicationResources.SubjectEdit_ParameterEdit;
            }
        }

        private void ChangeProperty(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SubItemsInsertUpdateMode")
            {
                if (SubItemsInsertUpdateMode)
                {
                    if (SelectedSubItemIndex == -1 && SelectedItemIndex == -1)
                    {
                        BtnAddButtonContent = ApplicationResources.SubjectInsert_ParameterInsert;
                    }
                    else if (SelectedSubItemIndex == -1 && SelectedItemIndex > -1)
                    {
                        BtnAddButtonContent = ApplicationResources.SubjectEdit_ParameterInsert;
                    }
                }
                else
                {
                    UnselectSubItem();
                    BtnAddButtonContent = SelectedItemIndex > -1
                        ? ApplicationResources.SubjectEdit
                        : ApplicationResources.SubjectInsert;
                }
            }
            if (e.PropertyName == "ApplyForAllStudyYears")
            {
                Classes = !ApplyForAllStudyYears ? new List<int> { 1, 2, 3, 4, 5 } : null;
            }
            if (e.PropertyName == "SelectedSubItem")
            {
                if (SelectedSubItem.Id == 0)
                {
                    ItemToDelete = SelectedItem;
                    BtnDeleteContent = ApplicationResources.DeleteSubject;
                }
                else
                {
                    ItemToDelete = SelectedSubItem;
                    BtnDeleteContent = ApplicationResources.DeleteSchedule;
                }
            }
            if (e.PropertyName == "SelectedItem")
            {
                if (SelectedItem.Id == 0)
                {
                    ItemToDelete = new { Id = (long)0 };
                }
                else
                {
                    ItemToDelete = SelectedItem;
                    BtnDeleteContent = ApplicationResources.DeleteSubject;
                }
            }
        }

        private void SelectSubItem(SubjectParameters subItem)
        {
            if (SelectedSubItemIndex != -1 && subItem.Id != SelectedSubItem.Id)
            {
                
            }
        }

        public void UnselectSubItem()
        {
            ApplyForAllStudyYears = false;
            SelectedSubItem = new SubjectParameters();
            SelectedSubItemIndex = -1;
            BtnAddButtonContent = SubItemsInsertUpdateMode
                ? ApplicationResources.SubjectEdit_ParameterInsert
                : ApplicationResources.SubjectEdit;
        }

        public void UnselectItem()
        {
            UnselectSubItem();
            SelectedItem = new Subject();
            SelectedItemIndex = -1;
            BtnAddButtonContent = SubItemsInsertUpdateMode
                ? ApplicationResources.SubjectInsert_ParameterInsert
                : ApplicationResources.SubjectInsert;
        }

        public void SelectItem(Subject item)
        {
            if (SelectedItemIndex != -1 && item.Id != SelectedItem.Id)
            {
                
            }
        }

        public void BindItems()
        {
            var rep = new ArtCollegeGenericDataRepository<Subject>();
            ItemsList =
                new ObservableCollection<Subject>(
                    (rep.GetAll(la => la.HoursParameters, la => la.HoursParameters.Select(p => p.Type))).OrderBy(
                        la => la.Name));

            if (SelectedItem.Id != 0)
            {
                SelectedItem = ItemsList.SingleOrDefault(la => la.Id == SelectedItem.Id).DeepClone();
            }

            var rep1 = new ArtCollegeGenericDataRepository<SubjectParameterType>();
            SubjectParameterTypes = new ObservableCollection<SubjectParameterType>(rep1.GetAll());
        }

        public void DeleteItem(Subject item)
        { }

        public void DeleteItemOrSubItem(object item)
        {
            var subject = item as Subject;
            if (subject != null)
            {
                var rep = new SubjectRepository();
                rep.Remove(subject);
                UnselectItem();
                BindItems();
                _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
                _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.SubjectDeleted);
            }
            else
            {
                var items = item as SubjectParameters;
                if (items != null)
                {
                    var rep = new SubjectParametersRepository();
                    rep.Remove(items);
                    UnselectSubItem();
                    BindItems();
                    _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
                    SelectedItemIndex = ItemsList.IndexOf(ItemsList.SingleOrDefault(la => la.Id == SelectedItem.Id));
                    _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.ScheduleDeleted);
                }
            }
        }

        public void AddUpdateItem(Subject item)
        {
            var repSubject = new SubjectRepository();
            var subjects = repSubject.GetAll();

            if (subjects.Any(la => (la.Name == item.Name || item.ShortName == la.ShortName) && (la.Id != item.Id || item.Id == 0)))
            {
                ErrorPopupViewRequest.Raise(new Notification
                {
                    Content = ApplicationResources.SubjectError,
                    Title = ApplicationResources.SubjectErrorTitle
                });
                return;
            }

            if (SubItemsInsertUpdateMode)
            {
                if (SelectedSubItem.Type.Id == 0)
                {
                    ErrorPopupViewRequest.Raise(new Notification
                    {
                        Content = ApplicationResources.ScheduleErrorEmptyType,
                        Title = ApplicationResources.ScheduleErrorTitle
                    });
                    return;
                }
                if (item.HoursParameters.Any(la => la.Type.Id == SelectedSubItem.Type.Id && ((la.StudyYear == SelectedSubItem.StudyYear && la.Id!=SelectedSubItem.Id) || ApplyForAllStudyYears)))
                {
                    ErrorPopupViewRequest.Raise(new Notification
                    {
                        Content = ApplicationResources.ScheduleError,
                        Title = ApplicationResources.ScheduleErrorTitle
                    });
                    return;
                }
            }

            if (item.Id == 0)
            {
                if (SubItemsInsertUpdateMode)
                {
                    if (ApplyForAllStudyYears)
                    {

                        for (var i = 1; i <= 5; i++)
                        {
                            var paramClone = SelectedSubItem.DeepClone();
                            paramClone.StudyYear = i;
                            item.HoursParameters.Add(paramClone);
                        }
                    }
                    else
                    {
                        item.HoursParameters.Add(SelectedSubItem);
                    }
                }
                
                repSubject.Add(item);
                BindItems();
                _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
                SelectedItemIndex = ItemsList.IndexOf(ItemsList.SingleOrDefault(la => la.Id == item.Id));
                UnselectSubItem();
                _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.SubjectAdded);
                return;
            }
            if (SubItemsInsertUpdateMode)
            {
                if (SelectedSubItemIndex != -1)
                {
                    item.HoursParameters.Remove(item.HoursParameters.SingleOrDefault(la => la.Id == SelectedSubItem.Id));
                    item.HoursParameters.Add(SelectedSubItem);
                }
                else
                {
                    if (ApplyForAllStudyYears)
                    {
                        for (var i = 1; i <= 5; i++)
                        {
                            var paramClone = SelectedSubItem.DeepClone();
                            paramClone.StudyYear = i;
                            item.HoursParameters.Add(paramClone);
                        }
                    }
                    else
                    {
                        item.HoursParameters.Add(SelectedSubItem);
                    }
                }
            }
            
            repSubject.Update(item);
            BindItems();
            _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
            SelectedItemIndex = ItemsList.IndexOf(ItemsList.SingleOrDefault(la => la.Id == item.Id));
            UnselectSubItem();
            _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.SubjectEdited);
        }

        public void ReBindItems(object obj)
        {
            BindItems();
        }
        #endregion
    }
}