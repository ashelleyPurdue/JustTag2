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
using System.IO;
using System.Reflection;
using JustTag2.TagPallette;
using JustTag2.Tagging;

namespace JustTag2.Pages
{
    /// <summary>
    /// Interaction logic for EditTagsPage.xaml
    /// </summary>
    public partial class EditTagsPage : Page
    {
        private static string exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string dbPath = Path.Combine(exeFolder, "tag_pallet.json");

        public event EventHandler MovedBack;

        private FileSystemInfo file;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"> The file whose tags are being edited.</param>
        /// <param name="beforeSaving"> Callback to be ran before saving any changes </param>
        /// <param name="afterSaving"> Callback to be ran after saving any changes </param>
        public EditTagsPage(FileSystemInfo file)
        {
            InitializeComponent();
            MovedBack += EditTagsPage_MovedBack;

            // Select the file
            this.file = file;
            previewer.Source = file;

            // Load the tag pallet
            tagPallette.DataContext = TagDatabase.Load(dbPath);

            // Populate the tags textbox
            tagsTextbox.Tags = TagUtils.GetTags(file).ToList();
        }

        private void EditTagsPage_MovedBack(object sender, EventArgs e)
        {
            tagPallette.ViewModel.Save(dbPath);
            previewer.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Validate the input

            previewer.Close();
            TagUtils.SetTags(file, tagsTextbox.Tags.ToArray());
            MovedBack?.Invoke(this, null);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => MovedBack?.Invoke(this, null);
    }
}
