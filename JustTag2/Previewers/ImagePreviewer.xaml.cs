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

            // HACK: make the combo box visible when the mouse
            // is over the entire ImagePreviewer.  This is too
            // cumbersome to do in XAML.
            void setVis() => scrollModeBox.Visibility =
                IsMouseOver ? Visibility.Visible
                            : Visibility.Hidden;

            MouseEnter += (s, a) => setVis();
            MouseLeave += (s, a) => setVis();
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

        public async Task Open(FileSystemInfo file)
        {
            // TODO: Consider doing this in another thread?

            // Load the bitmap image and close the file handle
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(file.FullName);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            image.Source = bitmap;
        }

        // The file is loaded completely into memory and then
        // immediately closed in the "Open" method, so there's
        // nothing special we need to do to release the file.
        public async Task Close() => image.Source = null;
    }
}
