using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for VideoPreviewer.xaml
    /// </summary>
    public partial class VideoPreviewer : UserControl, IPreviewer
    {
        public VideoPlayerViewModel viewModel = new VideoPlayerViewModel();

        public VideoPreviewer()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public UserControl Control => this;

        public bool CanPreview(FileSystemInfo file)
        {
            // TODO: Find a way to reduce the duplicate code.
            string[] extensions = new[]
            {
                ".gif",
                ".gifv",
                ".webm",
                ".mpg",
                ".mpeg",
                ".wmv",
                ".mp4",
                ".mov"  // TODO: Other formats that I can't remember off the top of my head
            };

            return extensions.Contains(file.Extension.ToLower());
        }

        public void Close()
        {
            player.Close();
        }

        public void Open(FileSystemInfo file)
        {
            player.Source = new Uri(file.FullName);
            viewModel.Length = player.NaturalDuration.TimeSpan.TotalMilliseconds;
        }
    }

    public class VideoPlayerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double Length { get; set; }
        public double Position { get; set; }
    }
}
