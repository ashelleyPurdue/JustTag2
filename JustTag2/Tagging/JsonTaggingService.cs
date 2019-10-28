#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

using Newtonsoft.Json;

namespace JustTag2.Tagging
{
    public class JsonTaggingService : ITaggingService
    {
        private static readonly string[] EMPTY_TAGS = new string[] { };
        private const string DB_FNAME = ".jtfiletags";

        public IEnumerable<string> GetTags(FileSystemInfo file)
        {
            string dbPath = GetDbPath(file);

            if (!File.Exists(dbPath))
                return EMPTY_TAGS;

            var tagDict = ParseDb(dbPath);

            if (!tagDict.ContainsKey(file.Name))
                return EMPTY_TAGS;

            return tagDict[file.Name];
        }

        public FileSystemInfo SetTags(FileSystemInfo file, IEnumerable<string> tags)
        {
            string dbPath = GetDbPath(file);

            if (!File.Exists(dbPath))
                File.WriteAllText(dbPath, "{}");

            var tagDict = ParseDb(dbPath);

            if (tagDict.ContainsKey(file.Name))
                tagDict[file.Name] = tags.ToArray();
            else
                tagDict.Add(file.Name, tags.ToArray());

            // This is very inefficient; it's O(n), where n is the number of files.
            // TODO: Find a way to make it so we only write the line that was changed.
            WriteDb(dbPath, tagDict);
            return file;
        }

        private string GetDbPath(FileSystemInfo file) 
            => Path.Combine(file.ParentFolderPath(), DB_FNAME);

        /// <summary>
        /// Parses the database at the given path and returns a dictionary
        /// mapping file names to tags.
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        private Dictionary<string, string[]> ParseDb(string dbPath)
        {
            // TODO: If the file is already cached, reuse it.

            if (!File.Exists(dbPath))
                return new Dictionary<string, string[]>();

            string text = File.ReadAllText(dbPath);
            return JsonConvert.DeserializeObject<Dictionary<string, string[]>>(text);
        }

        private void WriteDb(string dbPath, Dictionary<string, string[]> tagDict)
        {
            string text = JsonConvert.SerializeObject(tagDict);
            File.WriteAllText(dbPath, text);
        }
    }
}
