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
using System.ComponentModel;

namespace JustTag2.Previewers
{
    /// <summary>
    /// Interaction logic for MainPreviewer.xaml
    /// </summary>
    public partial class MainPreviewer : UserControl
    {
        private static IPreviewer[] previewers = new IPreviewer[]
        {
            new ImagePreviewer(),
            new FallbackPreviewer()
        };

        public FileSystemInfo Source
        {
            get => currentFile;
            set => Open(value);
        }
        private FileSystemInfo currentFile;

        private IPreviewer currentPreviewer;

        public MainPreviewer()
        {
            InitializeComponent();

            // Add all of the previewers' controls, but make them hidden.
            foreach (var p in previewers)
            {
                p.Control.Visibility = Visibility.Collapsed;
                grid.Children.Add(p.Control);
            }
        }

        private void Open(FileSystemInfo file)
        {
            currentFile = file;

            // Close the old previewer
            if (currentPreviewer != null)
            {
                currentPreviewer.Close();
                currentPreviewer.Control.Visibility = Visibility.Hidden;
            }

            // Open the new one
            currentPreviewer = GetPreviewer(file);
            currentPreviewer.Open(file);
            currentPreviewer.Control.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Returns the previewer used to display the given file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private IPreviewer GetPreviewer(FileSystemInfo file) => previewers
            .Where(p => p.CanPreview(file))
            .First();
    }
}
