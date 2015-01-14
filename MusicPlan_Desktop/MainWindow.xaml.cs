using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MusicPlan.BLL;
using MusicPlan.BLL.Models;
using MusicPlan.DAL.Repository;
using MusicPlan_Desktop.Resources;

namespace MusicPlan_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BindInstruments();
            BindStudents();
        }

        private void BindStudents()
        {
            var rep = new ArtCollegeGenericDataRepository<Student>();
            var instrumentsRep = new ArtCollegeGenericDataRepository<Instrument>();
            LstBoxStudentInstruments.ItemsSource = instrumentsRep.GetAll();
            DgStudents.ItemsSource = rep.GetAll(la => la.Instruments);
        }

        private void BindInstruments()
        {
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            DgInstruments.ItemsSource = rep.GetAll();
        }

        private void BtnDeleteInstrument_OnClick(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)(sender)).CommandParameter;
            var rep = new ArtCollegeGenericDataRepository<Instrument>();
            rep.Remove(rep.GetSingle(la => la.Id == id));
            BindInstruments();
            ClearInstrumentFields();
        }

        private void BtnAddInstrument_Click(object sender, RoutedEventArgs e)
        {
            var commandParam = ((Button)(sender)).CommandParameter;
            if (commandParam == null)
            {
                var rep = new ArtCollegeGenericDataRepository<Instrument>();
                rep.Add(new Instrument
                {
                    Name = TxtInstrumentName.Text
                });
                BindInstruments();
                ClearInstrumentFields();
            }
            else
            {
                var id = (int)commandParam;
                var rep = new ArtCollegeGenericDataRepository<Instrument>();
                var itemForUpdate = rep.GetSingle(la => la.Id == id);
                itemForUpdate.Name = TxtInstrumentName.Text;
                rep.Update(itemForUpdate);
                BindInstruments();
                ClearInstrumentFields();
            }
        }

        private void BtnCancelInsertInstrument_Click(object sender, RoutedEventArgs e)
        {
            DgInstruments.UnselectAll();
            ClearInstrumentFields();
        }

        private void ClearInstrumentFields()
        {
            BtnAddInstrument.Content = ApplicationResources.ResourceManager.GetString("Insert");
            BtnAddInstrument.CommandParameter = null;
            TxtInstrumentName.Text = string.Empty;
        }

        private void DgInstruments_OnSelected(object sender, RoutedEventArgs e)
        {
            var item = (Instrument)((DataGrid)sender).SelectedItem;
            if (item != null)
            {
                TxtInstrumentName.Text = item.Name;
                BtnAddInstrument.Content = ApplicationResources.ResourceManager.GetString("Edit");
                BtnAddInstrument.CommandParameter = item.Id;
            }
        }

        private void DgStudents_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            var item = (Student)((DataGrid)sender).SelectedItem;
            if (item != null)
            {
                TxtStudentFirstName.Text = item.FirstName;
                TxtStudentLastName.Text = item.LastName;
                TxtStudentMiddleName.Text = item.MiddleName;
                ComboStudyYears.SelectedValue = item.StudyYear;

                foreach (ListBoxItem lstItem in LstBoxStudentInstruments.Items)
                {
                    //if (lstItem.)
                    //{

                    //}
                }

                BtnAddInstrument.Content = ApplicationResources.ResourceManager.GetString("Edit");
                BtnAddInstrument.CommandParameter = item.Id;
            }
        }

        private void BtnDeleteStudent_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnAddStudent_Click(object sender, RoutedEventArgs e)
        {
            var commandParam = ((Button)(sender)).CommandParameter;
            if (commandParam == null)
            {
                var rep = new ArtCollegeGenericDataRepository<Student>();
                var itemToAdd = new Student
                {
                    FirstName = TxtStudentFirstName.Text,
                    LastName = TxtStudentLastName.Text,
                    MiddleName = TxtStudentMiddleName.Text,
                    StudyYear = int.Parse((string) ((ComboBoxItem) ComboStudyYears.SelectedValue).Content),
                    Instruments = LstBoxStudentInstruments.SelectedItems.Cast<Instrument>().ToList()
                };
                rep.Add(itemToAdd);
            }
            else
            {
                var id = (int)commandParam;
                var rep = new ArtCollegeGenericDataRepository<Student>();
                var itemForUpdate = rep.GetSingle(la => la.Id == id);
                itemForUpdate.FirstName = TxtStudentFirstName.Text;
                itemForUpdate.LastName = TxtStudentLastName.Text;
                itemForUpdate.MiddleName = TxtStudentMiddleName.Text;
                itemForUpdate.StudyYear = (int)ComboStudyYears.SelectedValue;
                itemForUpdate.Instruments = (ICollection<Instrument>) LstBoxStudentInstruments.SelectedItems;
                rep.Update(itemForUpdate);
            }

            BindStudents();
            ClearStudentFields();
        }

        private void BtnCancelInsertStudent_Click(object sender, RoutedEventArgs e)
        {
            DgStudents.UnselectAll();
            ClearStudentFields();
        }

        private void ClearStudentFields()
        {
            TxtStudentFirstName.Text = string.Empty;
            TxtStudentLastName.Text = string.Empty;
            TxtStudentMiddleName.Text = string.Empty;
            ComboStudyYears.SelectedIndex = -1;
            LstBoxStudentInstruments.UnselectAll();
        }
    }

    [ValueConversion(typeof(ICollection<Instrument>), typeof(string))]
    public class InstrumentsListToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.Join(string.Format("{0}", Environment.NewLine), ((ICollection<Instrument>)value).Select(la => la.Name).ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
