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
                Open(value);
            }
        }

        private static IPreviewer[] previewers = new IPreviewer[]
        {
            new ImagePreviewer(),
            new FallbackPreviewer()
        };

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

        /// <summary>
        /// Closes the currently open file
        /// </summary>
        public void Close() => currentPreviewer?.Close();

        private void Open(FileSystemInfo file)
        {
            // Close the old previewer
            if (currentPreviewer != null)
            {
                currentPreviewer.Close();
                currentPreviewer.Control.Visibility = Visibility.Hidden;
            }

            // Don't open new one if it's null
            if (file == null)
                return;

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
