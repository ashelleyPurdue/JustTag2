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
using TabbedFileBrowser;

namespace JustTag2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            browser.ViewModel.ParseFilterString = Tagging.TagUtils.ParseFilterString;

            // TODO: replace this with a databinding in XAML
            browser.ViewModel.PropertyChanged += (s, a) =>
                previewer.Source = browser.ViewModel.SelectedFile;
        }

        private void EditTagsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditTagsWindow(browser.ViewModel.SelectedFile);

            window.ShowDialog();
            browser.ViewModel.CurrentTab.Refresh();
        }
    }
}
