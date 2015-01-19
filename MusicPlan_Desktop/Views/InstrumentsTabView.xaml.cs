using System.Windows;
using System.Windows.Controls;
using MusicPlan_Desktop.ViewModels;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace MusicPlan_Desktop.Views
{
    /// <summary>
    /// Description for InstrumentsTabView.
    /// </summary>
    public partial class InstrumentsTabView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the InstrumentsTabView class.
        /// </summary>
        public InstrumentsTabView(InstrumentsViewModel instrumentsViewModel)
        {
            InitializeComponent();
            this.DataContext = instrumentsViewModel;
        }
    }
}