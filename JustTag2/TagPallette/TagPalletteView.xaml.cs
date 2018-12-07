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
using IllusoryStudios.Wpf.LostControls;

namespace JustTag2.TagPallette
{
    /// <summary>
    /// Interaction logic for TagPalletteView.xaml
    /// </summary>
    public partial class TagPalletteView : UserControl
    {
        public TagDatabase viewModel = PlaceholderTagDatabase.instance;

        public TagPalletteView()
        {
            InitializeComponent();
            DataContext = viewModel;
        }


        // Misc methods

        /// <summary>
        /// Shorthand for (T)(sender as Control).DataContext
        /// </summary>
        /// <param name="sender"></param>
        private T GetData<T>(object sender)
            => (T)(sender as Control).DataContext;


        // Event handlers

        private void Tag_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            // Start dragging the tag
            var s = sender as Control;
            string tag = s.Tag as string;

            DragDrop.DoDragDrop(s, tag + " ", DragDropEffects.Copy);
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            var category = GetData<TagCategory>(sender);

            category.Tags.Add(new TagPallette.Tag()
            {
                Name = $"Tag {category.Tags.Count}",
                Desc = ""
            });
        }

        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
