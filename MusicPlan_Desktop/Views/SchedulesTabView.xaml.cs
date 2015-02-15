using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
using MusicPlan_Desktop.Behaviors;
using MusicPlan_Desktop.Resources;
using MusicPlan_Desktop.ViewModels;

namespace MusicPlan_Desktop.Views
{
    /// <summary>
    /// Interaction logic for SchedulesTabView.xaml
    /// </summary>
    public partial class SchedulesTabView : UserControl
    {
        private static DataGrid MainDataGrid { get; set; }
        public SchedulesTabView(SchedulesMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        private void DataGrid_Initialized(object sender, EventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                MainDataGrid = sender as DataGrid;
                if (MainDataGrid != null)
                    dpd.AddValueChanged(MainDataGrid, DataGrid_OnItemSourceChanged);
            }
            BindColumns(sender);
        }

        private void DataGrid_OnItemSourceChanged(object sender, EventArgs e)
        {
            BindColumns(sender);
        }

        private static void BindColumns(object sender)
        {
            if (MainDataGrid == null) return;
            MainDataGrid.Columns.Clear();
            var source = MainDataGrid.ItemsSource as DataView;
            if (source == null) return;
            var columns = source.Table.Columns;

            var parserContext = new ParserContext();
            parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            parserContext.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            var resourcesType = typeof(ApplicationResources);
            parserContext.XmlnsDictionary.Add("resources", string.Format("clr-namespace:{0};assembly={1}", resourcesType.Namespace, resourcesType.Assembly.FullName));

            var studTemplateString = "<DataGridTextColumn IsReadOnly=\"True\" Binding=\"{Binding [0].Student.DisplayName}\" Width=\"*\" Header=\"{x:Static resources:ApplicationResources.Student}\" />";
            var textColumn = XamlReader.Parse(studTemplateString, parserContext) as DataGridTextColumn;
            MainDataGrid.Columns.Add(textColumn);

            studTemplateString = "<DataGridTextColumn IsReadOnly=\"True\" Binding=\"{Binding [0].Instrument.Name}\" Width=\"*\" Header=\"{x:Static resources:ApplicationResources.Instrument}\" />";
            textColumn = XamlReader.Parse(studTemplateString, parserContext) as DataGridTextColumn;
            MainDataGrid.Columns.Add(textColumn);

            for (var i = 1; i < columns.Count; i++)
            {
                //TODO selections
                var strTemplate =
                    string.Format("<DataTemplate>" +
                                  "<ListBox MaxHeight=\"100\" VirtualizingPanel.ScrollUnit=\"Pixel\" ItemsSource=\"{{Binding Path=[{0}].Subject.Teachers}}\" SelectionMode=\"Multiple\" " +
                                  "Width=\"220\" DisplayMemberPath=\"DisplayName\"> " +
                                  "<i:Interaction.Behaviors> <behaviors:SynchronizeSelectedListBoxItems " +
                                  "Selections=\"{{Binding Path=[{0}].Selections}}\" /></i:Interaction.Behaviors>" +
                                  "</ListBox>" +
                                  "</DataTemplate>", i);

                parserContext = new ParserContext();
                parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                parserContext.XmlnsDictionary.Add("i", "http://schemas.microsoft.com/expression/2010/interactivity");
                var listBoxSelectionType = typeof(SynchronizeSelectedListBoxItems);
                parserContext.XmlnsDictionary.Add("behaviors", string.Format("clr-namespace:{0};assembly={1}", listBoxSelectionType.Namespace, listBoxSelectionType.Assembly.FullName));
                var datatemplate = XamlReader.Parse(strTemplate, parserContext) as DataTemplate;
                var dgtc = new DataGridTemplateColumn
                {
                    Header = columns[i].ColumnName,
                    CellTemplate = datatemplate,
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                };
                MainDataGrid.Columns.Add(dgtc);
            }
        }

        private void Row_MouseEnter(object sender, MouseEventArgs e)
        {
            MainDataGrid.SelectedIndex = MainDataGrid.ItemContainerGenerator.IndexFromContainer((DataGridRow)sender);
        }

        private void DataGrid_OnMouseLeave(object sender, MouseEventArgs e)
        {
            MainDataGrid.SelectedIndex = -1;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var a = 0;
        }
    }
}