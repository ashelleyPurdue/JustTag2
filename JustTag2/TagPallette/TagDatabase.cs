using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTag2.TagPallette
{
    public class TagDatabase
    {
        public List<TagCategory> categories;

        public TagDatabase(string filePath) => throw new NotImplementedException();

        public void Save(string filePath) => throw new NotImplementedException();
        public void Load(string filePath) => throw new NotImplementedException();
    }

    public class TagCategory
    {
        public string name;
        public string desc;

        public List<Tag> tags;
    }

    public class Tag
    {
        public string name;
        public string desc;
    }
}
