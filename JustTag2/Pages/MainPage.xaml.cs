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

namespace JustTag2.Pages
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

        private void EditTagsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileSystemInfo file = ViewModel.SelectedFile;
            
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

            // Navigate to the page.
            window.Content = page;
        }
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate IComparable SortMethod(FileSystemInfo f);

        private static Random randGen = new Random();

        public Dictionary<string, SortMethod> SortMethods { get; set; } = new Dictionary<string, SortMethod>
        {
            {"Name", f => f.Name },
            {"Date", f => f.LastWriteTime },
            {"Shuffle", f => randGen.Next() }
        };

        public DirectoryInfo CurrentFolder { get; set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
        public IEnumerable<FileSystemInfo> VisibleFiles { get; set; }
        public FileSystemInfo SelectedFile { get; set; }

        public void Refresh()
        {
            VisibleFiles = CurrentFolder.EnumerateFileSystemInfos();    // TODO: Filtering and sorting
        }
    }
}
