using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace JustTag2.Tagging
{
    public class LegacyTaggingService : ITaggingService
    {
        private static Regex tagAreaRegex;
        static LegacyTaggingService()
        {
            const string STUFF_INSIDE_BRACKETS = @"\[.*\]";
            const string FILE_EXTENSION = @"\..*$";
            tagAreaRegex = new Regex
            (
                STUFF_INSIDE_BRACKETS +
                "(?=(" + FILE_EXTENSION + "))"
            );
        }

        /// <summary>
        /// Returns the tags on the given file or folder
        /// </summary>
        public IEnumerable<string> GetTags(FileSystemInfo file) => file switch
        {
            DirectoryInfo d => GetTags(d),
            FileInfo f => GetTags(f),
            _ => throw new Exception("Received a FileSystemInfo that is neither a directory nor a file.  WTF?")
        };

        private IEnumerable<string> GetTags(FileInfo file)
        {
            string fname = file.Name;

            // Find the tag area
            int tagAreaStart = fname.IndexOf('[');
            int tagAreaEnd = fname.IndexOf(']');
            int tagAreaLen = tagAreaEnd - tagAreaStart;

            // If no tag area was found, then there are no tags.
            if (tagAreaStart < 0)
                return new string[] { };

            // Error handling
            if (tagAreaEnd < 0)
                throw new Exception("Could not find ending ']'");

            if (tagAreaLen < 0)
                throw new Exception("ending ']' came before opening '['");

            // Extract the tags from the tag area.
            string tagArea = fname.Substring(tagAreaStart, tagAreaLen)
                                  .TrimStart('[')
                                  .TrimEnd(']');

            return tagArea.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private IEnumerable<string> GetTags(DirectoryInfo folder)
        {
            // If no .jtfoldertags exists, then there are no tags.
            string jtfoldertagsPath = Path.Combine(folder.FullName, ".jtfoldertags");
            if (!File.Exists(jtfoldertagsPath))
                return new string[] { };

            // All of the folder's tags are stored in its .jtfoldertags file.
            // They are separated by newlines.
            return File.ReadAllLines(jtfoldertagsPath);
        }

        /// <summary>
        /// Sets the tags on the given file or folder.
        /// Returns a new FileSystemInfo with the updated tags.
        /// </summary>
        public FileSystemInfo SetTags(FileSystemInfo file, IEnumerable<string> tags)
        {
            if (file is FileInfo f)
                return SetTags(f, tags);

            if (file is DirectoryInfo d)
                return SetTags(d, tags);

            throw new Exception("Did you create a new class that derives from FileSystemInfo?  WHY WOULD YOU DO THAT???");
        }

        private FileSystemInfo SetTags(FileInfo file, IEnumerable<string> tags)
        {
            // TODO: Find a nicer way to do this.

            // Get just the name, stripping off the tags and the extension
            string removedTags = tagAreaRegex.Replace(file.Name, "");
            string removedExt = new string
            (
                removedTags
                .Reverse()
                .SkipWhile(c => c != '.')
                .Skip(1)
                .Reverse()
                .ToArray()
            );

            // Create the tag area
            var tagArea = new StringBuilder();
            tagArea.Append('[');
            tagArea.AppendJoin(' ', tags);
            tagArea.Append(']');

            // Put 'em all together to get the new file name
            string finalName = removedExt + tagArea.ToString() + file.Extension;
            string finalPath = Path.Combine(file.DirectoryName, finalName);

            file.MoveTo(finalPath);
            return file;
        }

        private FileSystemInfo SetTags(DirectoryInfo dir, IEnumerable<string> tags)
        {
            string jtfoldertags = Path.Combine(dir.FullName, ".jtfoldertags");
            File.WriteAllLines(jtfoldertags, tags);

            return dir;
        }
    }
}
