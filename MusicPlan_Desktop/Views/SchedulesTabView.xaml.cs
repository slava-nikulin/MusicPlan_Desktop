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
            var dataGrid = (DataGrid) sender;
            var source = dataGrid.ItemsSource as DataTable;
            if(source == null) return;
            foreach (SubjectScheduleViewModel subj in source.Rows)
            {
                
                StringReader stringReader = new StringReader(
                    string.Format("<DataTemplate " +
    "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" +
    "xmlns:i=\"http://schemas.microsoft.com/expression/2010/interactivity\""+
            "<ListBox MaxHeight=\"100\" VirtualizingPanel.ScrollUnit=\"Pixel\" ItemsSource=\"{{Binding Subject.Teachers}}\" SelectionMode=\"Multiple\""+
                                   "Width=\"220\" DisplayMemberPath=\"DisplayName\"> <i:Interaction.Behaviors> <behaviors:SynchronizeSelectedListBoxItems " +
                                   "Selections=\"{{Binding Selections}}\" /></i:Interaction.Behaviors></ListBox></DataTemplate>")
    );
                XmlReader xmlReader = XmlReader.Create(stringReader);
                var datatemplate = XamlReader.Load(xmlReader) as DataTemplate;


                var context = new ParserContext();

                //context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
                //context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
                //context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

                context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                context.XmlnsDictionary.Add("i", "http://schemas.microsoft.com/expression/2010/interactivity");
                //context.XmlnsDictionary.Add("vm", "vm");
                //context.XmlnsDictionary.Add("v", "v");

                //var template = (DataTemplate)XamlReader.Parse(xaml, context);
                

                DataGridTemplateColumn dgc = new DataGridTemplateColumn();
                dgc.Header = ApplicationResources.Teachers;


                //dgc.CellTemplate = template;
                dataGrid.Columns.Add(dgc);
            }
        }
    }
}