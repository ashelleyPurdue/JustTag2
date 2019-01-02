using System;
using System.Collections.Generic;
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
        public MainPage()
        {
            InitializeComponent();
            browser.ViewModel.ParseFilterString = Tagging.TagUtils.ParseFilterString;

            // Set the sorting options
            var randGen = new Random();
            browser.ViewModel.SortMethods = new Dictionary<string, TabbedFileBrowser.SortMethod>()
            {
                {"Name", f => f.Name },
                {"Date", f => f.LastWriteTime },
                {"Shuffle", f => randGen.Next() }
            };

            // TODO: replace this with a databinding in XAML
            browser.ViewModel.PropertyChanged += (s, a) =>
                previewer.Source = browser.ViewModel.SelectedFile;
        }

        private void EditTagsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileSystemInfo file = browser.ViewModel.SelectedFile;
            
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
                browser.ViewModel.CurrentTab.Refresh();
                previewer.Source = file;    // re-open the same file(even if it no longer appears in the file list)
            };

            // Navigate to the page.
            window.Content = page;
        }
    }
}
