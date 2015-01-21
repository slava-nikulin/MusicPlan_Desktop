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
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;
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
        private ObservableCollection<XamlListItem<Instrument>> _availableInstruments;

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
            get { return _selectedItem ?? new Student(); }
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

        public ObservableCollection<XamlListItem<Instrument>> AvailableInstruments
        {
            get { return _availableInstruments; }
            set { SetProperty(ref _availableInstruments, value); }
        }

        public ICommand AddUpdateCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand CancelSelectionCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }
        #endregion

        #region Constructor
        public StudentsViewModel()
        {
            PrepareViewModel();
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            AvailableInstruments = new ObservableCollection<XamlListItem<Instrument>>();
            foreach (var instr in rep.GetAll())
            {
                AvailableInstruments.Add(new XamlListItem<Instrument>
                {
                    IsSelected = false,
                    XListItem = instr
                });
            }
            
        }

        public void PrepareViewModel()
        {
            BindItems();
            SelectedItem = new Student();
            SelectedItemIndex = -1;
            AddUpdateCommand = new DelegateCommand<Student>(AddUpdateItem);
            DeleteItemCommand = new DelegateCommand<Student>(DeleteItem);
            SelectItemCommand = new DelegateCommand<Student>(SelectItem);
            CancelSelectionCommand = new DelegateCommand(UnselectItem);
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
        }

        #endregion

        #region ViewModel command handlers
        public void UnselectItem()
        {
            SelectedItem = new Student();
            SelectedItemIndex = -1;
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
            foreach (var instr in AvailableInstruments)
            {
                instr.IsSelected = false;
            }
        }

        public void SelectItem(Student item)
        {
            if (SelectedItemIndex != -1)
            {
                SelectedItem = (Student)item.DeepClone();
                foreach (var singleOrDefault in SelectedItem.Instruments.Select(instr => AvailableInstruments.SingleOrDefault(la => la.XListItem.Id == instr.Id))
                            .Where(singleOrDefault => singleOrDefault != null))
                {
                    singleOrDefault.IsSelected = true;
                }
                BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Edit");
            }
        }

        public void BindItems()
        {
            var rep = new ArtCollegeGenericDataRepository<Student>();
            ItemsList = new ObservableCollection<Student>(rep.GetAll(la=>la.Instruments));
        }

        public void DeleteItem(Student item)
        {
            var rep = new ArtCollegeGenericDataRepository<Student>();
            rep.Remove(item);
            BindItems();
            UnselectItem();
        }

        public void AddUpdateItem(Student item)
        {
            if (item.Id == 0)
            {
                var rep = new ArtCollegeGenericDataRepository<Student>();
                rep.Add(item);
            }
            else
            {
                var rep = new ArtCollegeGenericDataRepository<Student>();
                rep.Update(item);
            }
            BindItems();
            UnselectItem();
        }
        #endregion
    }
}
