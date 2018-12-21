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
            var libDirectory = Path.Combine(currentDirectory, "ffmpeg");

            Unosquare.FFME.MediaElement.FFmpegDirectory = libDirectory;

            // Don't let FFME swallow exceptions.  That's bad juju
            player.MediaFailed += (s, a) => throw a.ErrorException;

            // Binding the time slider to the video position in XAML
            // turned out to be more complicated than just doing it in
            // C#.  Simplicity is king.
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
            player.Close();
        }

        public void Open(FileSystemInfo file)
        {
            player.Source = new Uri(file.FullName);
        }

        private void RedneckDatabindTimeSlider()
        {
            // Manually subscribe to time-change events and update
            // the slider.  And vice-versa.
            // Who needs XAML, anyway?

            // Bind the slider's value to the video's position
            bool skipTimeChangedEvent = false;
            bool skipValueChangedEvent = false;

            player.PositionChanged += (s, a) =>
            {
                if (skipTimeChangedEvent)
                    return;

                skipValueChangedEvent = true;
                timeSlider.Value = player.Position.Ticks;
                skipValueChangedEvent = false;
            };
            
            timeSlider.ValueChanged += (s, a) =>
            {
                if (skipValueChangedEvent)
                    return;

                skipTimeChangedEvent = true;
                player.Position = new TimeSpan((long)(timeSlider.Value));
                skipTimeChangedEvent = false;
            };

            player.MediaOpened += (s, a) =>
            {
                timeSlider.Maximum = player.MediaInfo.Duration.Ticks;
            };
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.IsPlaying)
                player.Pause();
            else if (player.IsPaused)
                player.Play();
        }
    }
}
