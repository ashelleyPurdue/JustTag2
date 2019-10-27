using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace JustTag2
{
    /// <summary>
    /// Provides methods for getting and setting tags on files/folders
    /// </summary>
    public interface ITaggingService
    {
        /// <summary>
        /// Returns the tags on the given file or folder
        /// </summary>
        IEnumerable<string> GetTags(FileSystemInfo file);

        /// <summary>
        /// Sets the tags on the given file or folder.
        /// Returns a new FileSystemInfo with the updated tags.
        /// </summary>
        FileSystemInfo SetTags(FileSystemInfo file, IEnumerable<string> tags);

        /// <summary>
        /// Produces a function that returns true if a file matches the given filter string,
        /// or false if it doesn't.
        /// </summary>
        public Func<FileSystemInfo, bool> ParseFilterString(string filterString)
        {
            // HACK: If no filter string is present, don't filter at all.
            if (filterString == null)
                return (f => true);

            // HACK: Show only untagged files if the string is ":untagged:"
            if (filterString == ":untagged:")
                return (f => !GetTags(f).Any());

            // Build a list of tags that are required/forbidden.
            // Forbidden tags have a '-' in front of them.
            string[] terms = filterString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var requiredTags = new List<string>();
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
                var tags = GetTags(f);

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
