﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace JustTag2.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            browser.ViewModel.ParseFilterString = Tagging.TagUtils.ParseFilterString;

            // TODO: replace this with a databinding in XAML
            browser.ViewModel.PropertyChanged += (s, a) =>
                previewer.Source = browser.ViewModel.SelectedFile;
        }

        private void EditTagsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileSystemInfo file = browser.ViewModel.SelectedFile;

            var page = new EditTagsPage(file);

            // Make sure the file is temporarily closed while the tags are
            // saved, so we don't get a file-in-use error.
            page.beforeSaving = previewer.Close;
            page.afterSaving = () => previewer.Source = file;

            // Navigate to the page.
            var window = Window.GetWindow(this);

            page.MovedBack += (s, a) =>
            {
                window.Content = this;
                browser.ViewModel.CurrentTab.Refresh();
            };
            window.Content = page;
            
        }
    }
}
