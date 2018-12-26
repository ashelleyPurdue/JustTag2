using System;
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

namespace JustTag2.Previewers
{
    /// <summary>
    /// Interaction logic for FolderPreviewer.xaml
    /// </summary>
    public partial class FolderPreviewer : UserControl, IPreviewer
    {
        private IPreviewer[] previewers = new IPreviewer[]
        {
            new ImagePreviewer(),
            new VideoPreviewer(),
            new FallbackPreviewer()
        };

        private MainPreviewerCore core;

        private FileSystemInfo[] browsableFiles;
        private int currentIndex = 0;

        public FolderPreviewer()
        {
            InitializeComponent();
            core = new MainPreviewerCore(previewerGrid, previewers);
        }

        public UserControl Control => this;
        public bool CanPreview(FileSystemInfo file) => file is DirectoryInfo;
        public Task Close() => core.Close();

        public async Task Open(FileSystemInfo file)
        {
            // TODO: Handle empty folders somehow

            var folder = (DirectoryInfo)file;

            // Load up a list of all the files in the given folder
            browsableFiles = folder.EnumerateFileSystemInfos().ToArray();
            currentIndex = 0;

            await OpenCurrent();
        }

        private async Task OpenCurrent()
        {
            // Make sure the index is between 0 and the number of files
            while (currentIndex < 0)
                currentIndex += browsableFiles.Length;

            while (currentIndex >= browsableFiles.Length)
                currentIndex -= browsableFiles.Length;

            // Open it
            await core.Open(browsableFiles[currentIndex]);
        }

        private async void prevButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex--;
            await OpenCurrent();
        }

        private async void nextButton_Click(object sender, RoutedEventArgs e)
        {
            currentIndex++;
            await OpenCurrent();
        }
    }
}
