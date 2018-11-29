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

namespace JustTag2.Previewers
{
    /// <summary>
    /// Interaction logic for MainPreviewer.xaml
    /// </summary>
    public partial class MainPreviewer : UserControl
    {
        private IPreviewer[] previewers = new IPreviewer[]
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
                AddChild(p.Control);
            }
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
