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
using System.Windows.Shapes;
using System.IO;
using JustTag2.Tagging;

namespace JustTag2
{
    /// <summary>
    /// Interaction logic for EditTagsWindow.xaml
    /// </summary>
    public partial class EditTagsWindow : Window
    {
        private FileSystemInfo file;

        public EditTagsWindow(FileSystemInfo file)
        {
            InitializeComponent();

            this.file = file;

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

            TagUtils.SetTags(file, tags);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
            => this.Close();
    }
}
