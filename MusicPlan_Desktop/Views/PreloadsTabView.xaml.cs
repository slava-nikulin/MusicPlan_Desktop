﻿using System;
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

namespace MusicPlan_Desktop.Views
{
    /// <summary>
    /// Interaction logic for PreloadsTabView.xaml
    /// </summary>
    public partial class PreloadsTabView : UserControl
    {
        public PreloadsTabView(PreloadViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
