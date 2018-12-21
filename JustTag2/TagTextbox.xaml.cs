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
using System.Windows.Shapes;

namespace JustTag2
{
    /// <summary>
    /// Interaction logic for TagTextbox.xaml
    /// </summary>
    public partial class TagTextbox : UserControl
    {
        public IEnumerable<string> Tags
        {
            get => tags;
            set
            {
                tags = value.ToList();
                tagsList.ItemsSource = tags;
            }
        }
        private List<string> tags;

        public TagTextbox()
        {
            InitializeComponent();
        }

        private void TypingBox_KeyDown(object sender, KeyEventArgs e)
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
                tags.Add(tag);
                typingBox.Clear();
            }
        }
    }
}
