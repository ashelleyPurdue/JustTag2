using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PropertyChanged;

namespace JustTag2
{
    /// <summary>
    /// Interaction logic for TagTextbox.xaml
    /// </summary>
    public partial class TagTextbox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> Tags { get; set; }

        public TagTextbox()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void RefreshListview()
        {
            var old = Tags;
            Tags = null;
            Tags = old;
        }

        private void TypingBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // If the user types whitespace, add the tag they were typing to the list
            var whitespaceKeys = new Key[]
            {
                Key.Space,
                Key.Tab,
                Key.Enter
            };

            if (whitespaceKeys.Contains(e.Key))
            {
                e.Handled = true;

                string tag = typingBox.Text;
                Tags.Add(tag);
                typingBox.Clear();

                RefreshListview();
            }

            // Delete the previous tag when the user hits backspace
            if (typingBox.Text == "" && e.Key == Key.Back && Tags.Count > 0)
            {
                Tags.RemoveAt(Tags.Count - 1);
                RefreshListview();
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete the tag associated with this button
            // NOTE: This implementation will delete the
            // first occurrance of this tag.  This is fine
            // if all tags are unique, but if the user adds
            // the same tag more than once, then it will seem
            // weird to them.

            var button = (Button)sender;
            Tags.Remove((string)button.Tag);

            RefreshListview();
        }
    }
}
