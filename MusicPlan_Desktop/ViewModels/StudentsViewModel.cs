﻿using System;
using System.Collections;
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
using MusicPlan_Desktop.Modules;
using MusicPlan_Desktop.Resources;

namespace MusicPlan_Desktop.ViewModels
{
    public class StudentsViewModel : BindableBase, ICrudViewModel<Student>
    {
        #region Private fields
        private Student _selectedItem;
        private ObservableCollection<Student> _itemsList;
        private string _btnAddButtonContent;
        private int _selectedItemIndex;
        private ObservableCollection<Instrument> _availableInstruments;
        private readonly ObservableCollection<Instrument> _selections = new ObservableCollection<Instrument>();
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

        public Student SelectedItem
        {
            get { return _selectedItem ?? new Student{Instruments = new List<Instrument>()}; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public ObservableCollection<Student> ItemsList
        {
            get { return _itemsList; }
            set { SetProperty(ref _itemsList, value); }
        }
        public string BtnAddButtonContent
        {
            get { return _btnAddButtonContent; }
            set { SetProperty(ref _btnAddButtonContent, value); }
        }

        public List<int> Classes
        {
            get { return new List<int> { 1, 2, 3, 4, 5 }; }
        }

        public ObservableCollection<Instrument> AvailableInstruments
        {
            get { return _availableInstruments; }
            set { SetProperty(ref _availableInstruments, value); }
        }

        public IList Selections
        {
            get { return  _selections; }
        }

        public ICommand AddUpdateCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand CancelSelectionCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }
        #endregion

        #region Constructor
        public StudentsViewModel(IUnityContainer container)
        {
            _container = container;
            _eventAggregator = _container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<SyncDataEvent>().Subscribe(ReBindItems, true);
            PrepareViewModel();
        }

        public void PrepareViewModel()
        {
            ReBindItems(null);
            SelectedItem = new Student();
            SelectedItemIndex = -1;
            AddUpdateCommand = new DelegateCommand<Student>(AddUpdateItem);
            DeleteItemCommand = new DelegateCommand<Student>(DeleteItem);
            SelectItemCommand = new DelegateCommand<Student>(SelectItem);
            CancelSelectionCommand = new DelegateCommand(UnselectItem);
            BtnAddButtonContent = ApplicationResources.Insert;
        }

        #endregion

        #region ViewModel methods
        public void ReBindItems(object obj)
        {
            BindItems();
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            AvailableInstruments = new ObservableCollection<Instrument>(rep.GetAll().OrderBy(la => la.Name));
        }

        public void UnselectItem()
        {
            SelectedItem = new Student();
            SelectedItemIndex = -1;
            BtnAddButtonContent = ApplicationResources.Insert;
            Selections.Clear();
        }

        public void SelectItem(Student item)
        {
            if (SelectedItemIndex != -1 && item.Id!= SelectedItem.Id)
            {
                SelectedItem = item.DeepClone();
                Selections.Clear();
                foreach (var instr in item.Instruments)
                {
                    Selections.Add(AvailableInstruments.Single(la=>la.Id == instr.Id));
                }
                BtnAddButtonContent = ApplicationResources.Edit;
            }
        }

        public void BindItems()
        {
            var rep = new StudentRepository();
            ItemsList =
                new ObservableCollection<Student>(
                    rep.GetAll(la => la.Instruments, la => la.StudentToTeachers).OrderBy(la => la.LastName));
        }

        public void DeleteItem(Student item)
        {
            var rep = new StudentRepository();
            rep.Remove(item);
            BindItems();
            UnselectItem();
            _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
            _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.StudentDeleted);
        }

        public void AddUpdateItem(Student item)
        {
            item.Instruments = new List<Instrument>((IEnumerable<Instrument>) Selections);
            var rep = new StudentRepository();
            if (item.Id == 0)
            {
                rep.Add(item);
                _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.StudentAdded);
            }
            else
            {
                rep.Update(item);
                _eventAggregator.GetEvent<ShowStatusMessageEvent>().Publish(ApplicationResources.StudentEdited);
            }
            BindItems();
            UnselectItem();
            _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
        }
        #endregion
    }
}
