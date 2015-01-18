using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;

namespace MusicPlan_Desktop.ViewModels
{
    public class InstrumentsViewModel : BindableBase
    {
        private Instrument _selectedInstrument;
        private ObservableCollection<Instrument> _instrumentsList;
        private string _btnAddInstrumentContent;
        private string _btnAddInstrumentCommand;

        public Instrument SelectedInstrument
        {
            get { return _selectedInstrument; }
            set
            {
                SetProperty(ref _selectedInstrument, value);
            }
        }
        public ObservableCollection<Instrument> InstrumentsList
        {
            get { return _instrumentsList; }
            set
            {
                SetProperty(ref _instrumentsList, value);
            }
        }
        public string BtnAddInstrumentContent
        {
            get { return _btnAddInstrumentContent; }
            set { SetProperty(ref _btnAddInstrumentContent, value); }
        }
        public string BtnAddInstrumentCommand
        {
            get { return _btnAddInstrumentCommand; }
            set { SetProperty(ref _btnAddInstrumentCommand, value); }
        }

        public ICommand AddUpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelSelectionCommand { get; set; }
        public ICommand SelectInstrumentCommand { get; set; }

        public InstrumentsViewModel()
        {
            BindInstruments();
            SelectedInstrument = new Instrument();
            AddUpdateCommand = new DelegateCommand<int?>(AddUpdateInstrument);
            DeleteCommand = new DelegateCommand<int?>(DeleteInstrument);
        }

        private void BindInstruments()
        {
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            InstrumentsList = new ObservableCollection<Instrument>(rep.GetAll());
        }

        private void DeleteInstrument(int? id)
        {
            //var id = (int)((Button)(sender)).CommandParameter;
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            rep.Remove(rep.GetSingle(la => la.Id == id));
            BindInstruments();
            //BindStudents();
            //ClearInstrumentFields();
        }

        private void AddUpdateInstrument(int? id)
        {
            //if (id == 0)
            //{
            //    var rep = new ArtCollegeGenericDataRepository<Instrument>();
            //    rep.Add(new Instrument
            //    {
            //        Name = TxtInstrumentName.Text
            //    });
            //}
            //else
            //{
            //    var rep = new ArtCollegeGenericDataRepository<Instrument>();
            //    var itemForUpdate = rep.GetSingle(la => la.Id == id);
            //    itemForUpdate.Name = TxtInstrumentName.Text;
            //    rep.Update(itemForUpdate);
            //}
            //BindInstruments();
            //BindStudents();
            //ClearInstrumentFields();
        }

    }
}