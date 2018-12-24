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

namespace JustTag2.Previewers
{
    /// <summary>
    /// Interaction logic for FolderPreviewer.xaml
    /// </summary>
    public partial class FolderPreviewer : UserControl, IPreviewer
    {
        public FolderPreviewer()
        {
            InitializeComponent();
        }

        public UserControl Control => this;

        public bool CanPreview(FileSystemInfo file)
        {
            throw new NotImplementedException();
        }

        public Task Close()
        {
            throw new NotImplementedException();
        }

        public Task Open(FileSystemInfo file)
        {
            throw new NotImplementedException();
        }
    }
}
