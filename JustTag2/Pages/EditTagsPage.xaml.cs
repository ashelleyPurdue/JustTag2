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
using System.IO;
using System.Reflection;
using JustTag2.Tagging;
using System.ComponentModel;
using System.Collections.ObjectModel;
using AutoPropertyChanged;

namespace JustTag2.Views
{
    /// <summary>
    /// Interaction logic for EditTagsPage.xaml
    /// </summary>
    public partial class EditTagsPage : Page
    {
        private static string exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string dbPath = Path.Combine(exeFolder, "tag_pallet.json");

        public event EventHandler MovedBack;

        private EditTagsPageViewModel ViewModel;
        private readonly ITaggingService _taggingService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"> The file whose tags are being edited.</param>
        /// <param name="beforeSaving"> Callback to be ran before saving any changes </param>
        /// <param name="afterSaving"> Callback to be ran after saving any changes </param>
        public EditTagsPage(FileSystemInfo file, ITaggingService taggingService)
        {
            _taggingService = taggingService;

            InitializeComponent();
            MovedBack += EditTagsPage_MovedBack;

            addTagTextbox.Focus();

            // Fill out the view model
            ViewModel = new EditTagsPageViewModel(taggingService);
            DataContext = ViewModel;

            ViewModel.File = file;
            ViewModel.TagDatabase = TagDatabase.Load(dbPath);

            // Open the file
            previewer.Source = file;
        }


        // Misc methods

        private string GetSenderTag(object sender) => (string)((Control)sender).Tag;

        // Event handlers

        private async void EditTagsPage_MovedBack(object sender, EventArgs e)
        {
            ViewModel.TagDatabase.Save(dbPath);
            await previewer.Close();
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            await previewer.Close();
            _taggingService.SetTags(ViewModel.File, ViewModel.Tags.ToArray());
            MovedBack?.Invoke(this, null);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => MovedBack?.Invoke(this, null);

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Click "OK" if it's shift-enter
            bool shiftHeld = Keyboard.IsKeyDown(Key.LeftShift) ||
                             Keyboard.IsKeyDown(Key.RightShift);

            if (e.Key == Key.Enter && shiftHeld)
                OK_Click(sender, null);
        }

        private void AddTagTextbox_EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            var textbox = (TextBox)sender;

            ViewModel.Tags.Add(textbox.Text);   // TODO: Validate this input
            textbox.Text = "";
        }

        private void TagAddButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = GetSenderTag(sender);
            ViewModel.Tags.Add(tag);
        }

        private void TagRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = GetSenderTag(sender);
            ViewModel.Tags.Remove(tag);
        }

    }

    public class EditTagsPageViewModel : INotifyPropertyChanged
    {
        private readonly ITaggingService _taggingService;

        public FileSystemInfo File
        {
            get => file;
            set
            {
                file = value;
                Tags = new ObservableCollection<string>(_taggingService.GetTags(file));
            }
        }
        private FileSystemInfo file;

        public EditTagsPageViewModel(ITaggingService taggingService)
        {
            _taggingService = taggingService;
        }

        [NotifyChanged] public TagDatabase TagDatabase { get; set; }
        [NotifyChanged] public ObservableCollection<string> Tags { get; set; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
