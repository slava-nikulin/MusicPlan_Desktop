using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
    public class TeachersViewModel : BindableBase, ICrudViewModel<Teacher>
    {
        #region Private fields
        private Teacher _selectedItem;
        private ObservableCollection<Teacher> _itemsList;
        private string _btnAddButtonContent;
        private int _selectedItemIndex;
        private ObservableCollection<Subject> _availableSubjects;
        private readonly ObservableCollection<Subject> _selections = new ObservableCollection<Subject>();
        private IEventAggregator _eventAggregator;
        #endregion

        #region Constructor
        public TeachersViewModel(IUnityContainer container)
        {
            _eventAggregator = container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<SyncDataEvent>().Subscribe(ReBindItems, true);
            PrepareViewModel();
        }

        public void PrepareViewModel()
        {
            ReBindItems(null);
            SelectedItem = new Teacher();
            SelectedItemIndex = -1;
            AddUpdateCommand = new DelegateCommand<Teacher>(AddUpdateItem);
            DeleteItemCommand = new DelegateCommand<Teacher>(DeleteItem);
            SelectItemCommand = new DelegateCommand<Teacher>(SelectItem);
            CancelSelectionCommand = new DelegateCommand(UnselectItem);
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
        }
        #endregion

        #region Public properties

        public IList Selections
        {
            get { return _selections; }
        }

        public ObservableCollection<Subject> AvailableSubjects
        {
            get { return _availableSubjects; }
            set { SetProperty(ref _availableSubjects, value); }
        }

        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set { SetProperty(ref _selectedItemIndex, value); }
        }

        public Teacher SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public ObservableCollection<Teacher> ItemsList
        {
            get { return _itemsList; }
            set { SetProperty(ref _itemsList, value); }
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

        #region ViewModel methods

        public void UnselectItem()
        {
            SelectedItem = new Teacher();
            SelectedItemIndex = -1;
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
            Selections.Clear();
        }

        public void SelectItem(Teacher item)
        {
            if (SelectedItemIndex != -1 && item.Id != SelectedItem.Id)
            {
                SelectedItem = item.DeepClone();
                Selections.Clear();
                foreach (var subj in item.Subjects)
                {
                    Selections.Add(AvailableSubjects.Single(la => la.Id == subj.Id));
                }
                BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Edit");
            }
        }

        public void BindItems()
        {
            var rep = new ArtCollegeGenericDataRepository<Teacher>();
            ItemsList = new ObservableCollection<Teacher>(rep.GetAll(la => la.Subjects).OrderBy(la=>la.LastName));
        }

        public void DeleteItem(Teacher item)
        {
            var rep = new TeacherRepository();
            rep.Remove(item);
            BindItems();
            UnselectItem();
            _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
        }

        public void AddUpdateItem(Teacher item)
        {
            item.Subjects = new List<Subject>((IEnumerable<Subject>) Selections);
            var rep = new TeacherRepository();
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
            _eventAggregator.GetEvent<SyncDataEvent>().Publish(null);
        }

        public void ReBindItems(object obj)
        {
            BindItems();
            var rep = new ArtCollegeGenericDataRepository<Subject>();
            AvailableSubjects = new ObservableCollection<Subject>(rep.GetAll().OrderBy(la => la.Name));
        }
        #endregion
    }
}
