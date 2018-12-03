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
    /// Interaction logic for ImagePreviewer.xaml
    /// </summary>
    public partial class ImagePreviewer : UserControl, IPreviewer
    {
        public UserControl Control => this;

        public ImagePreviewer()
        {
            InitializeComponent();
        }

        public bool CanPreview(FileSystemInfo file)
        {
            string[] imageFormats = new[]
            {
                ".png",
                ".jpg",
                ".jpeg",
                ".tiff",
                ".bmp",
                ".webp",
                ".bpg"  // Yes, this is real.  Check wikipedia!
            };

            return imageFormats.Contains(file.Extension.ToLower());
        }

        public void Open(FileSystemInfo file)
        {
            image.Source = new BitmapImage(new Uri(file.FullName));
        }

        public void Close() => image.Source = null;
    }
}
