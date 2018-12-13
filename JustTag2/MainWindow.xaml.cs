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
using TabbedFileBrowser;

namespace JustTag2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            browser.ViewModel.ParseFilterString = Tagging.TagUtils.ParseFilterString;

            // TODO: replace this with a databinding in XAML
            browser.ViewModel.PropertyChanged += (s, a) =>
                previewer.Source = browser.ViewModel.SelectedFile;
        }

        private void EditTagsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FileSystemInfo file = browser.ViewModel.SelectedFile;

            var window = new EditTagsWindow(file);

            // Make sure the file is temporarily closed while the tags are
            // saved, so we don't get a file-in-use error.
            window.beforeSaving = previewer.Close;
            window.afterSaving  = () => previewer.Source = file;

            window.ShowDialog();
            browser.ViewModel.CurrentTab.Refresh();
        }
    }
}
