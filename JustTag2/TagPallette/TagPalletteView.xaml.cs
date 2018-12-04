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

namespace JustTag2.TagPallette
{
    /// <summary>
    /// Interaction logic for TagPalletteView.xaml
    /// </summary>
    public partial class TagPalletteView : UserControl
    {
        public TagDatabase viewModel = PlaceholderTagDatabase.instance;

        public TagPalletteView()
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
