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

namespace JustTag2.Views
{
    /// <summary>
    /// Interaction logic for MainPreviewer.xaml
    /// </summary>
    public partial class MainPreviewer : UserControl
    {
        public static readonly DependencyProperty sourceProperty = DependencyProperty.Register
        (
            "Source",
            typeof(FileSystemInfo),
            typeof(MainPreviewer),
            new PropertyMetadata((s, a) =>
            {
                ((MainPreviewer)s).Source = (FileSystemInfo)a.NewValue;
            })
        );
        public FileSystemInfo Source
        {
            get => (FileSystemInfo)GetValue(sourceProperty);
            set
            {
                SetValue(sourceProperty, value);
                core.Open(value);
            }
        }

        private IPreviewer[] previewers = new IPreviewer[]
        {
            new FolderPreviewer(),
            new ImagePreviewer(),
            new VideoPreviewer(),
            new FallbackPreviewer()
        };


        private MainPreviewerCore core;

        public MainPreviewer()
        {
            InitializeComponent();
            core = new MainPreviewerCore(grid, previewers);
        }

        public Task Close() => core.Close();
    }

    /// <summary>
    /// Why is this logic separated out into a different class?
    /// Because I need to reuse it in FolderPreviewer, and I want
    /// to avoid using inheritance for that.
    /// </summary>
    public class MainPreviewerCore
    {
        private IPreviewer[] previewers;
        private IPreviewer currentPreviewer;

        public MainPreviewerCore(Grid grid, IPreviewer[] previewers)
        {
            this.previewers = previewers;

            // Add all of the previewers' controls, but make them hidden.
            foreach (var p in previewers)
            {
                p.Control.Visibility = Visibility.Collapsed;
                grid.Children.Add(p.Control);
            }
        }

        /// <summary>
        /// Closes the currently open file
        /// </summary>
        public Task Close() => currentPreviewer?.Close();

        public async Task Open(FileSystemInfo file)
        {
            // Close the old previewer
            if (currentPreviewer != null)
            {
                await currentPreviewer.Close();
                currentPreviewer.Control.Visibility = Visibility.Hidden;
            }

            // Don't open new one if it's null
            if (file == null)
                return;

            // Open the new one
            currentPreviewer = GetPreviewer(file);
            await currentPreviewer.Open(file);
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
