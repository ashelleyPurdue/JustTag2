using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using AutoPropertyChanged;

namespace JustTag2
{
    public class TagDatabase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyChanged] public ObservableCollection<TagCategory> Categories { get; set; }

        public TagDatabase() { }

        public void Save(string filePath)
        {
            string output = JsonConvert.SerializeObject(this, Formatting.Indented);
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
            TagDatabase database = JsonConvert.DeserializeObject<TagDatabase>(contents);

            // HACK: Alphabetize all the tags after loading
            foreach (TagCategory c in database.Categories)
                c.SortTags();

            return database;
        }
    }

    public class TagCategory : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyChanged] public string Name { get; set; }
        [NotifyChanged] public string Desc { get; set; }

        [NotifyChanged] public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();

        public void SortTags()
        {
            var sorted = Tags.OrderBy(t => t.Name);
            Tags = new ObservableCollection<Tag>(sorted);
        }

        public override string ToString() => Name;
    }

    public class Tag : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyChanged] public string Name { get; set; }
        [NotifyChanged] public string Desc { get; set; }

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
