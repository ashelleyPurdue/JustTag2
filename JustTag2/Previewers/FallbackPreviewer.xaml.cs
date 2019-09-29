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

namespace JustTag2.Views
{
    /// <summary>
    /// Interaction logic for FallbackPreviewer.xaml
    /// </summary>
    public partial class FallbackPreviewer : UserControl, IPreviewer
    {
        public FallbackPreviewer()
        {
            InitializeComponent();
        }

        public UserControl Control => this;

        public bool CanPreview(FileSystemInfo file) => true;

        public async Task Close() { }   // The file was never actually opened to begin with, so nothing to close.

        public async Task Open(FileSystemInfo file)
        {
            fileNameLabel.Text = file.Name;
        }
    }
}
