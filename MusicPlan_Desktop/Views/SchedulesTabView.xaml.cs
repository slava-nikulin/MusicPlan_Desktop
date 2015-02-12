using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using MusicPlan_Desktop.Resources;
using MusicPlan_Desktop.ViewModels;

namespace MusicPlan_Desktop.Views
{
    /// <summary>
    /// Interaction logic for SchedulesTabView.xaml
    /// </summary>
    public partial class SchedulesTabView : UserControl
    {
        public SchedulesTabView(SchedulesMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void FrameworkElement_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GetValue(sender);
        }

        private void DataGrid_Initialized(object sender, EventArgs e)
        {
            GetValue(sender);
        }
        
        private static void GetValue(object sender)
        {
            //var dataGrid = (DataGrid) sender;
            //var source = dataGrid.ItemsSource as DataView;
            //if (source == null) return;
            //foreach (DataColumn col in source.Table.Columns.Cast<DataColumn>().Skip(2))
            //{
            //    var strTemplate =
            //        string.Format("<DataTemplate>" +
            //                      "<ListBox MaxHeight=\"100\" VirtualizingPanel.ScrollUnit=\"Pixel\" ItemsSource=\"{{Binding Path=[{0}].Subject.Teachers}}\" SelectionMode=\"Multiple\" " +
            //                      "Width=\"220\" DisplayMemberPath=\"DisplayName\"> <i:Interaction.Behaviors> <behaviors:SynchronizeSelectedListBoxItems " +
            //                      "Selections=\"{{Binding Path=[{0}].Selections}}\" /></i:Interaction.Behaviors>" +
            //                      "</ListBox>" +
            //                      "</DataTemplate>",
            //            col.ColumnName);
            //    var parserContext = new ParserContext();
            //    parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            //    parserContext.XmlnsDictionary.Add("i", "http://schemas.microsoft.com/expression/2010/interactivity");
            //    parserContext.XmlnsDictionary.Add("behaviors", "clr-namespace:MusicPlan_Desktop.Behaviors");
            //    var datatemplate = XamlReader.Parse(strTemplate, parserContext) as DataTemplate;
            //    DataGridTemplateColumn dgtc = new DataGridTemplateColumn
            //    {
            //        Header = col.ColumnName,
            //        CellTemplate = datatemplate
            //    };
            //    dataGrid.Columns.Add(dgtc);
            //}
        }
    }
}