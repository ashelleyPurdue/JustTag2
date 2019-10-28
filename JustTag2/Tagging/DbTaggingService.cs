#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JustTag2.Tagging
{
    public class DbTaggingService : ITaggingService
    {
        private static readonly string[] EMPTY_TAGS = new string[] { };
        private const string DB_FNAME = ".jtfiletags";

        public IEnumerable<string> GetTags(FileSystemInfo file)
        {
            string dbPath = Path.Combine(file.ParentFolderPath(), DB_FNAME);

            if (!File.Exists(dbPath))
                return EMPTY_TAGS;

            // TODO: If the database file is in the cache, reuse it
            // TODO: If the database file is not in the cache, parse it
            var tagDict = ParseDb(dbPath);

            if (!tagDict.ContainsKey(file.Name))
                return EMPTY_TAGS;

            return tagDict[file.Name];
        }

        public FileSystemInfo SetTags(FileSystemInfo file, IEnumerable<string> tags)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses the database at the given path and returns a dictionary
        /// mapping file names to tags.
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        private Dictionary<string, string[]> ParseDb(string dbPath)
        {
            // The file is structured like this:
            // file1.txt:tag1,tag2,tag3
            // file2.txt:tag1,tag2,tag3
            // etc.
            // Spaces are valid in the file name, because unfortunately Windows
            // lets you put spaces in file names.

            var fileTags = new Dictionary<string, string[]>();
            var lines = File.ReadAllLines(dbPath);

            foreach (string line in lines)
            {
                var fnameTagsSplit = line.Split(':');
                string fname  = fnameTagsSplit[0];
                string[] tags = fnameTagsSplit[1].Split(',');

                fileTags.Add(fname, tags);
            }

            return fileTags;
        }
    }
}
