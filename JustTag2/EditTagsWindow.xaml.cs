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
using JustTag2.Pages;

namespace JustTag2
{
    /// <summary>
    /// Interaction logic for EditTagsWindow.xaml
    /// </summary>
    public partial class EditTagsWindow : Window
    {
        public Action beforeSaving
        {
            set => page.beforeSaving = value;
            get => page.beforeSaving;
        }

        public Action afterSaving
        {
            set => page.afterSaving = value;
            get => page.afterSaving;
        }


        private EditTagsPage page;

        public EditTagsWindow(FileSystemInfo file)
        {
            InitializeComponent();
            page = new EditTagsPage(file);
            this.Content = page;
        }
    }
}
