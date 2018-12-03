using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTag2.TagPallette
{
    public class TagDatabase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TagCategory> Categories { get; set; }

        public TagDatabase(string filePath) => throw new NotImplementedException();

        public void Save(string filePath) => throw new NotImplementedException();
        public void Load(string filePath) => throw new NotImplementedException();
    }

    public class TagCategory : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Desc { get; set; }

        public ObservableCollection<Tag> Tags { get; set; }
    }

    public class Tag : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
