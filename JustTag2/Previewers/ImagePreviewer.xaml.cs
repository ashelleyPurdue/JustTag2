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

namespace JustTag2.Views
{
    /// <summary>
    /// Interaction logic for ImagePreviewer.xaml
    /// </summary>
    public partial class ImagePreviewer : UserControl, IPreviewer
    {
        public UserControl Control => this;
        public enum ScrollMode { Stretch, Scroll };

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

            // HACK: Change the scroll mode.  This is SO not
            // using MVVM
            scrollModeBox.ItemsSource = Enum.GetValues(typeof(ScrollMode));
            scrollModeBox.SelectionChanged += (s, a) => RefreshScrollMode();
            scrollModeBox.SelectedIndex = 0;    // If we forget to do this, then we get a
                                                // swallowed exception when we try to access
                                                // SelectedItem.

            // HACK: Don't make the window shake when the user
            // scrolls to the end of the image with touch inputs
            scrollViewer.ManipulationBoundaryFeedback += (s, a) =>
                a.Handled = true;
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
            RefreshScrollMode();

            // Reset scroll bars so the user doesn't need to scroll back
            // to the top every time
            scrollViewer.ScrollToTop();
            scrollViewer.ScrollToLeftEnd();
        }

        // The file is loaded completely into memory and then
        // immediately closed in the "Open" method, so there's
        // nothing special we need to do to release the file.
        public async Task Close() => image.Source = null;

        private void RefreshScrollMode()
        {
            var scrollMode = (ScrollMode)scrollModeBox.SelectedItem;

            // Don't let it scroll, make it stretch.
            if (scrollMode == ScrollMode.Stretch)
            {
                image.Stretch = Stretch.Uniform;
                scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                scrollViewer.PanningMode = PanningMode.None;

                return;
            }

            image.Stretch = Stretch.UniformToFill;
            PickScrollDirection();
        }

        private void PickScrollDirection()
        {
            ScrollBarVisibility ToVis(bool value) =>
                value ? ScrollBarVisibility.Visible
                      : ScrollBarVisibility.Disabled;

            double width = image.Source.Width;
            double height = image.Source.Height;

            scrollViewer.HorizontalScrollBarVisibility = ToVis(width >= height);
            scrollViewer.VerticalScrollBarVisibility   = ToVis(width <= height);

            if (width > height)
                scrollViewer.PanningMode = PanningMode.HorizontalOnly;
            else if (width < height)
                scrollViewer.PanningMode = PanningMode.VerticalOnly;
            else
                scrollViewer.PanningMode = PanningMode.Both;
        }

        private void ScrollViewer_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {

        }
    }
}
