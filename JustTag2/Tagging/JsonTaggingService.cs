#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO.Abstractions;

using Newtonsoft.Json;

namespace JustTag2.Tagging
{
    public class JsonTaggingService : ITaggingService
    {
        private static readonly string[] EMPTY_TAGS = new string[] { };
        private const string DB_FNAME = ".jtfiletags";

        private readonly IFileSystem _fs;
        private IFile File => _fs.File;
        private IPath Path => _fs.Path;

        private string? _cachedDbPath;
        private Dictionary<string, string[]>? _cachedDb;

        public JsonTaggingService(IFileSystem fs)
        {
            _fs = fs;
        }

        public IEnumerable<string> GetTags(System.IO.FileSystemInfo file)
        {
            string dbPath = GetDbPath(file);

            if (!File.Exists(dbPath))
                return EMPTY_TAGS;

            var tagDict = ParseDb(dbPath);

            if (!tagDict.ContainsKey(file.Name))
                return EMPTY_TAGS;

            return tagDict[file.Name];
        }

        public System.IO.FileSystemInfo SetTags(System.IO.FileSystemInfo file, IEnumerable<string> tags)
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

        public IEnumerable<System.IO.FileSystemInfo> GetMatchingFiles(System.IO.DirectoryInfo dir, TagFilter filter)
        {
            IDirectoryInfo dirInfo = _fs.DirectoryInfo.FromDirectoryName(dir.FullName);
            return dirInfo
                .EnumerateFileSystemInfos()
                .Select(UnAbstract)
                .Select(f => (file: f, tags: GetTags(f)))
                .Where(pair => filter(pair.tags))
                .Select(pair => pair.file);
        }

        private System.IO.FileSystemInfo UnAbstract(IFileSystemInfo f) => f switch
        {
            IDirectoryInfo d => new System.IO.DirectoryInfo(f.FullName) as System.IO.FileSystemInfo,
            _ => new System.IO.FileInfo(f.FullName)
        };

        private string GetDbPath(System.IO.FileSystemInfo file) 
            => Path.Combine(file.ParentFolderPath(), DB_FNAME);

        /// <summary>
        /// Parses the database at the given path and returns a dictionary
        /// mapping file names to tags.
        /// </summary>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        private Dictionary<string, string[]> ParseDb(string dbPath)
        {
            // If the file doesn't exist, we can just use a blank dictionary
            if (!File.Exists(dbPath))
                return new Dictionary<string, string[]>();

            // If the file is already cached, reuse it.
            if (_cachedDbPath == dbPath && _cachedDb != null)
                return _cachedDb;

            // It's not cached, so load it and save it in the cache
            // before returning it.
            _cachedDbPath = dbPath;
            string text = File.ReadAllText(dbPath);
            if (text == "")
                _cachedDb = new Dictionary<string, string[]>();
            else
                _cachedDb = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(text);

            return _cachedDb;
        }

        private void WriteDb(string dbPath, Dictionary<string, string[]> tagDict)
        {
            string text = JsonConvert.SerializeObject(tagDict);
            File.WriteAllText(dbPath, text);
        }
    }
}
