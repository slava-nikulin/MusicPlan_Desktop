using System;
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
using MusicPlan_Desktop.ViewModels;
using MusicPlan.BLL.Models;

namespace MusicPlan_Desktop.Views
{
    /// <summary>
    /// Interaction logic for StudentsTabView.xaml
    /// </summary>
    public partial class StudentsTabView : UserControl
    {
        public StudentsTabView(StudentsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
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
