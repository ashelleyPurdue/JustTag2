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
using JustTag2.Tagging;

namespace JustTag2.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private MainPageViewModel ViewModel = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();
            DataContext = ViewModel;

            ViewModel.Refresh();

            // TODO: replace this with a databinding in XAML
            ViewModel.PropertyChanged += (s, a) =>
                previewer.Source = ViewModel.SelectedFile;
        }

        private void OpenEditTagsPage(FileSystemInfo file)
        {
            var window = Window.GetWindow(this);
            var page = new EditTagsPage(file);

            // Make sure the file is temporarily closed while the tags are
            // edited, so we don't get a file-in-use error when saving.
            previewer.Close();

            // Tell the edit page to return here when the back
            // button is pressed.
            page.MovedBack += (s, a) =>
            {
                window.Content = this;
                ViewModel.Refresh();
                previewer.Source = file;    // re-open the same file(even if it no longer appears in the file list)
            };

            window.Content = page;
        }

        private void EditTagsMenuItem_Click(object sender, RoutedEventArgs e)
            => OpenEditTagsPage(ViewModel.SelectedFile);


        private bool isSwiping = false;
        private TouchDevice mainFinger; // The finger whose speed we'll be measuring.
        private double lastSwipePos;
        private double lastSwipeSpeed = 0;
        private System.Diagnostics.Stopwatch swipeTimer = new System.Diagnostics.Stopwatch();

        private void Previewer_TouchDown(object sender, TouchEventArgs e)
        {
            // Cancel a swipe if the user uses more than one finger.
            // This way the user can safely scroll with 2 fingers without
            // accidentally swiping.
            if (previewer.TouchesOver.Count() > 1)
            {
                isSwiping = false;
                return;
            }

            // Start swiping
            mainFinger = e.TouchDevice;
            isSwiping = true;
            lastSwipePos = e.GetTouchPoint(previewer).Position.Y;
            swipeTimer.Restart();
        }

        private void Previewer_TouchUp(object sender, TouchEventArgs e)
        {
            if (!isSwiping) return;
            isSwiping = false;

            // Move to the next/previous file if the user swiped up or down
            int index = ViewModel.SelectedIndex;

            const double SWIPE_THRESHOLD = 1000;

            if (lastSwipeSpeed > SWIPE_THRESHOLD)
                ViewModel.SwipeToNextFile(false);

            if (lastSwipeSpeed < -SWIPE_THRESHOLD)
                ViewModel.SwipeToNextFile(true);
        }

        private void Previewer_TouchMove(object sender, TouchEventArgs e)
        {
            if (!isSwiping) return;
            if (e.TouchDevice != mainFinger) return;

            // Update the swipe speed
            double currentPos = e.GetTouchPoint(previewer).Position.Y;
            double deltaPos = currentPos - lastSwipePos;
            double deltaTime = swipeTimer.Elapsed.TotalSeconds;

            lastSwipeSpeed = deltaPos / deltaTime;

            // Record things for the next frame
            swipeTimer.Restart();
            lastSwipePos = currentPos;
        }


        private double initialStylusPos;
        private double finalStylusPos;
        private void Previewer_StylusDown(object sender, StylusDownEventArgs e)
        {
            initialStylusPos = e.GetPosition(previewer).Y;
        }

        private void Previewer_StylusUp(object sender, StylusEventArgs e)
        {
            finalStylusPos = e.GetPosition(previewer).Y;
        }

        private void Previewer_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
        {
            // Move to the previous/next file if they swipe with the stylus
            if (e.SystemGesture != SystemGesture.Flick)
                return;

            bool previous = finalStylusPos < initialStylusPos;
            ViewModel.SwipeToNextFile(previous);
        }

        private void VisibleFilesList_EditTagsClicked(object sender, FileSystemInfo f)
            => OpenEditTagsPage(f);
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate IComparable SortMethod(FileSystemInfo f);

        private static Random randGen = new Random();

        public int SelectedSortMethodIndex { get; set; } = 0;
        public Dictionary<string, SortMethod> SortMethods { get; set; } = new Dictionary<string, SortMethod>
        {
            {"Name", f => f.Name },
            {"Date", f => f.LastWriteTime },
            {"Shuffle", f => randGen.Next() }
        };

        public DirectoryInfo CurrentFolder { get; set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
        public FileSystemInfo[] VisibleFiles { get; set; }
        public bool SortDescending { get; set; } = true;

        public int SelectedIndex { get; set; }
        public FileSystemInfo SelectedFile => (SelectedIndex < VisibleFiles.Length && SelectedIndex >= 0)
            ? VisibleFiles[SelectedIndex]
            : null;

        public string FilterString { get; set; }

        /// <summary>
        /// Selects the next file.  If previous is true, moves
        /// to the previous file instead
        /// </summary>
        /// <param name="previous"></param>
        public void SwipeToNextFile(bool previous)
        {
            int index = SelectedIndex;

            if (previous)
                index--;
            else
                index++;

            if (index < 0)
                index = VisibleFiles.Length - 1;
            if (index >= VisibleFiles.Length)
                index = 0;

            SelectedIndex = index;
        }

        public void Refresh()
        {
            var filter = TagUtils.ParseFilterString(FilterString);
            var sortMethodKey = SortMethods.Keys.ToArray()[SelectedSortMethodIndex];
            var sortMethod = SortMethods[sortMethodKey];

            IEnumerable<FileSystemInfo> visibleFiles = CurrentFolder
                .EnumerateFiles()
                .Where(filter)
                .OrderBy(f => sortMethod(f));

            IEnumerable<FileSystemInfo> visibleFolders = CurrentFolder
                .EnumerateDirectories()
                .Where(filter)
                .OrderBy(f => sortMethod(f));   // TODO: Do something about this copypasta

            if (!SortDescending)
            {
                visibleFiles   = visibleFiles.Reverse();
                visibleFolders = visibleFolders.Reverse();
            }

            VisibleFiles = visibleFolders
                .Concat(visibleFiles)
                .ToArray();
        }
    }
}
