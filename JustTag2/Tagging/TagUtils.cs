using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabbedFileBrowser;
using System.IO;

namespace JustTag2.Tagging
{
    public static class TagUtils
    {
        /// <summary>
        /// Returns the tags on the given file or folder
        /// </summary>
        public static string[] GetTags(FileSystemInfo file)
        {
            if (file is DirectoryInfo d)
                return GetTags(d);
            else if (file is FileInfo f)
                return GetTags(f);
            else
                throw new Exception("Received a FileSystemInfo that is neither a directory nor a file.  WTF?");
        }

        private static string[] GetTags(FileInfo file)
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

        private static string[] GetTags(DirectoryInfo folder)
        {
            // TODO: Actually implement this.
            return new string[] { };
        }

        /// <summary>
        /// Sets the tags on the given file or folder.
        /// Returns a new FileSystemInfo with the updated tags.
        /// </summary>
        public static FileSystemInfo SetTags(FileSystemInfo file, string[] tags) => throw new NotImplementedException();

        /// <summary>
        /// Produces a function that returns true if a file matches the given filter string,
        /// or false if it doesn't.
        /// </summary>
        public static FilterCondition ParseFilterString(string filterString)
        {
            // HACK: If no filter string is present, don't filter at all.
            if (filterString == null)
                return (f => true);

            // HACK: Show only untagged files if the string is ":untagged:"
            if (filterString == ":untagged:")
                return (f => GetTags(f).Length == 0);

            // Build a list of tags that are required/forbidden.
            // Forbidden tags have a '-' in front of them.
            string[] terms = filterString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var requiredTags  = new List<string>();
            var forbiddenTags = new List<string>();

            foreach (string s in terms)
            {
                if (s[0] == '-')
                    forbiddenTags.Add(s.TrimStart('-'));
                else
                    requiredTags.Add(s);
            }

            return (FileSystemInfo f) =>
            {
                string[] tags = GetTags(f);

                // Fail if any of the required tags are missing,
                foreach (string t in requiredTags)
                {
                    if (!tags.Contains(t))
                        return false;
                }

                // Fail if any of the forbidden tags are present
                foreach (string t in tags)
                {
                    if (forbiddenTags.Contains(t))
                        return false;
                }

                return true;
            };
        }
    }
}
