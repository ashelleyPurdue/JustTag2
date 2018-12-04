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

        public TagDatabase() { }
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

    public static class PlaceholderTagDatabase
    {
        public static TagDatabase instance = new TagDatabase()
        {
            Categories = new ObservableCollection<TagCategory>()
            {
                new TagCategory()
                {
                    Name = "File Type",
                    Desc = "The type of file this is",
                    Tags = new ObservableCollection<Tag>()
                    {
                        new Tag() {Name = "image", Desc = "A picture from the interwebz" },
                        new Tag() {Name = "video", Desc = ""},
                        new Tag() {Name = "text"}
                    }
                },

                new TagCategory()
                {
                    Name = "Franchise",
                    Desc = "",
                    Tags = new ObservableCollection<Tag>()
                    {
                        new Tag() {Name = "overwatch" },
                        new Tag() {Name = "mario" },
                        new Tag() {Name = "minecraft" }
                    }
                }
            }
        };
    }
}
