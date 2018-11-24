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

            string tagArea = fname.Substring(tagAreaStart, tagAreaLen);

            // Extract the tags from the tag area.
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
            string[] terms = filterString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var requiredTags  = new HashSet<string>();
            var forbiddenTags = new HashSet<string>();

            foreach (string s in terms)
            {
                if (s[0] == '-')
                    forbiddenTags.Add(s);
                else
                    requiredTags.Add(s);
            }

            return (FileSystemInfo f) =>
            {
                // Fail if any of the required tags are missing,
                // or if any of the forbidden tags are present.
                foreach (string tag in GetTags(f))
                {
                    if (!requiredTags.Contains(tag))
                        return false;

                    if (forbiddenTags.Contains(tag))
                        return false;
                }

                return true;
            };
        }
    }
}
