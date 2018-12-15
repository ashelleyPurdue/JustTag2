using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace JustTag2.Previewers
{
    /// <summary>
    /// Interaction logic for VideoPreviewer.xaml
    /// </summary>
    public partial class VideoPreviewer : UserControl, IPreviewer
    {
        public VideoPreviewer()
        {
            InitializeComponent();

            // Boilerplate needed to set up the video player
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            player.SourceProvider.CreatePlayer(libDirectory);

            // Because I'm too sleepy to figure out the XAML right now.
            RedneckDatabindTimeSlider();
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
            player.SourceProvider.MediaPlayer.SetMedia("");
        }

        public void Open(FileSystemInfo file)
        {
            var uri = new Uri(file.FullName);
            player.SourceProvider.MediaPlayer.Play(uri);
        }

        private void RedneckDatabindTimeSlider()
        {
            // Manually subscribe to time-change events and update
            // the slider.  And vice-versa.
            // Who needs XAML, anyway?
            var mediaPlayer = player.SourceProvider.MediaPlayer;

            mediaPlayer.LengthChanged += (s, a) =>
                Dispatcher.Invoke(() => timeSlider.Maximum = mediaPlayer.Length);   // Dispatcher.Invoke is needed because LengthChanged
                                                                                    // doesn't occur on the main thread

            bool timeChanging = false;  
            mediaPlayer.TimeChanged += (s, a) => Dispatcher.Invoke(() =>
            {
                timeChanging = true;
                timeSlider.Value = mediaPlayer.Time;
                timeChanging = false;
            });

            timeSlider.ValueChanged += (s, a) =>
            {
                // We don't want to cause an infinite loop of TimeChanged events 
                if (timeChanging)
                    return;     

                mediaPlayer.Time = (long)timeSlider.Value;
            };
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
