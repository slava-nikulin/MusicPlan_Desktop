using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;
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
        public InstrumentsViewModel()
        {
            PrepareViewModel();
        }
        #endregion

        #region ViewModel command handlers

        public void PrepareViewModel()
        {
            BindItems();
            SelectedItem = new Instrument();
            SelectedItemIndex = -1;
            AddUpdateCommand = new DelegateCommand<Instrument>(AddUpdateItem);
            DeleteItemCommand = new DelegateCommand<Instrument>(DeleteItem);
            SelectItemCommand = new DelegateCommand<Instrument>(SelectItem);
            CancelSelectionCommand = new DelegateCommand(UnselectItem);
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
        }

        public void UnselectItem()
        {
            SelectedItem = new Instrument();
            SelectedItemIndex = -1;
            BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Insert");
        }

        public void SelectItem(Instrument item)
        {
            if (SelectedItemIndex != -1)
            {
                SelectedItem = (Instrument)item.Clone();
                BtnAddButtonContent = ApplicationResources.ResourceManager.GetString("Edit");
            }
        }

        public void BindItems()
        {
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            ItemsList = new ObservableCollection<Instrument>(rep.GetAll());
        }

        public void DeleteItem(Instrument item)
        {
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            rep.Remove(item);
            BindItems();
            UnselectItem();
        }

        public void AddUpdateItem(Instrument item)
        {
            if (item.Id == 0)
            {
                var rep = new ArtCollegeGenericDataRepository<Instrument>();
                rep.Add(item);
            }
            else
            {
                var rep = new ArtCollegeGenericDataRepository<Instrument>();
                rep.Update(item);
            }
            BindItems();
            UnselectItem();
        }
        #endregion
    }
}