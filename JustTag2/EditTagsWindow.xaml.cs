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
using System.IO;
using System.Reflection;
using JustTag2.TagPallette;
using JustTag2.Tagging;

namespace JustTag2
{
    /// <summary>
    /// Interaction logic for EditTagsWindow.xaml
    /// </summary>
    public partial class EditTagsWindow : Window
    {
        private static string exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string dbPath = Path.Combine(exeFolder, "tag_pallet.json");

        private FileSystemInfo file;

        public Action beforeSaving;
        public Action afterSaving;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"> The file whose tags are being edited.</param>
        /// <param name="beforeSaving"> Callback to be ran before saving any changes </param>
        /// <param name="afterSaving"> Callback to be ran after saving any changes </param>
        public EditTagsWindow(FileSystemInfo file)
        {
            InitializeComponent();

            this.file = file;

            // Load the tag pallet
            tagPallette.DataContext = TagDatabase.Load(dbPath);

            // Populate the tags textbox
            string[] tags = TagUtils.GetTags(file);
            foreach (string t in tags)
                tagsTextbox.AppendText(t + "\n");
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Split the text into tags
            string[] tags = tagsTextbox.Text.Split
            (
                ' ',
                '\r',
                '\n',
                '\t'
            );

            // TODO: Validate the input

            beforeSaving?.Invoke();
            TagUtils.SetTags(file, tags);
            afterSaving?.Invoke();

            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => this.Close();

        private void Window_Closed(object sender, EventArgs e)
        {
            tagPallette.ViewModel.Save(dbPath);
        }
    }
}
