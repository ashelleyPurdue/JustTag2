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
using JustTag2.TagPallette;
using JustTag2.Tagging;
using System.ComponentModel;

namespace JustTag2.Pages
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"> The file whose tags are being edited.</param>
        /// <param name="beforeSaving"> Callback to be ran before saving any changes </param>
        /// <param name="afterSaving"> Callback to be ran after saving any changes </param>
        public EditTagsPage(FileSystemInfo file)
        {
            InitializeComponent();
            ViewModel = new EditTagsPageViewModel();
            DataContext = ViewModel;

            MovedBack += EditTagsPage_MovedBack;

            // Select the file
            ViewModel.file = file;
            previewer.Source = file;

            // Load the tag pallet
            ViewModel.tagDatabase = TagDatabase.Load(dbPath);

            // Populate the tags textbox
            tagsTextbox.Tags = TagUtils.GetTags(file).ToList();
        }

        private async void EditTagsPage_MovedBack(object sender, EventArgs e)
        {
            ViewModel.tagDatabase.Save(dbPath);
            await previewer.Close();
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Validate the input

            await previewer.Close();
            TagUtils.SetTags(ViewModel.file, tagsTextbox.Tags.ToArray());
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
    }

    public class EditTagsPageViewModel : INotifyPropertyChanged
    {
        public FileSystemInfo file { get; set; }
        public TagDatabase tagDatabase { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
