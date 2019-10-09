using System;
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
using System.IO;

using JustTag2.Views;

namespace JustTag2.Views
{
    /// <summary>
    /// Interaction logic for VisibleFilesList.xaml
    /// </summary>
    public partial class VisibleFilesList : UserControl
    {
        public MainPageViewModel ViewModel => (MainPageViewModel)DataContext;
        public event EventHandler<FileSystemInfo> EditTagsClicked;

        private FileSystemInfo rightClickedFile;

        public VisibleFilesList()
        {
            InitializeComponent();
        }

        private void FileItem_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2)
                return;

            FileSystemInfo item = (sender as FrameworkElement).Tag as FileSystemInfo;

            switch (item)
            {
                case FileInfo file:
                    // TODO: Open the file with its default program.
                    break;
                case DirectoryInfo dir:
                    ViewModel.CurrentFolder = dir;
                    ViewModel.Refresh();
                    break;
            }
        }

        private void FileItemRightClicked(object sender, ContextMenuEventArgs e)
        {
            // Find the index of the file 
            var control = (FrameworkElement)sender;
            rightClickedFile = (FileSystemInfo)control.Tag;
        }

        private void FileItemTripleDots_Clicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            button.ContextMenu.IsOpen = true;

            // Setting IsOpen to true doesn't cause FileItemRightClicked to fire,
            // so we need to update rightClickedFile here as well.
            rightClickedFile = (FileSystemInfo)button.Tag;
        }

        private void FileItemContextMenuEditTags_Click(object sender, RoutedEventArgs e)
            => EditTagsClicked?.Invoke(this, rightClickedFile);

        private void DeleteFileButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show
            (
                $"Are you sure you want to PERMANENTLY delete {rightClickedFile.Name}?",
                "You sure about that?",
                MessageBoxButton.YesNo
            );

            if (result != MessageBoxResult.Yes)
                return;

            switch (rightClickedFile)
            {
                case DirectoryInfo d: d.Delete(true); break;
                case FileInfo f: f.Delete(); break;
            }

            ViewModel.Refresh();
        }
    }
}
