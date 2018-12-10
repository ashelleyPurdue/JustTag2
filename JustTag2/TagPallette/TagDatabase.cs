using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;

namespace JustTag2.TagPallette
{
    public class TagDatabase : INotifyPropertyChanged
    {
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TagCategory> Categories { get; set; }

        public TagDatabase() { }

        public void Save(string filePath)
        {
            string output = serializer.Serialize(this);
            File.WriteAllText(filePath, output);
        }

        /// <summary>
        /// Loads a database from the given path.
        /// If the path doesn't exist, returns a blank database(!!!)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static TagDatabase Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new TagDatabase()
                {
                    Categories = new ObservableCollection<TagCategory>()
                };
            }

            string contents = File.ReadAllText(filePath);
            return serializer.Deserialize<TagDatabase>(contents);
        }
    }

    public class TagCategory : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Desc { get; set; }

        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();

        public override string ToString() => Name;
    }

    public class Tag : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string Desc { get; set; }

        public override string ToString() => Name;
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
