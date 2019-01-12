using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using JustTag2.Util;

namespace JustTag2.Pages
{
    public interface IEditTagsPageViewModel
    {
        FileSystemInfo CurrentFile { get; }
        IList<string> Tags { get; }
    }

    public class EditTagsPageViewModel : IEditTagsPageViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FileSystemInfo CurrentFile { get; set; }
        public ObservableList<string> Tags { get; set; }
        IList<string> IEditTagsPageViewModel.Tags => this.Tags;

        public void Save()
        {
            
        }

    }
}
