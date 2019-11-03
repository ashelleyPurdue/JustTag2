using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JustTag2.Tests
{
    /// <summary>
    /// A flexible tagging service.
    /// Every method has a corresponding property you can use to
    /// supply the implementation.
    /// </summary>
    public class MockTaggingService : ITaggingService
    {
        public IEnumerable<string> GetTags(FileSystemInfo file)
            => GetTagsImpl(file);

        public FileSystemInfo SetTags(FileSystemInfo file, IEnumerable<string> tags)
            => SetTagsImpl(file, tags);

        public IEnumerable<FileSystemInfo> GetMatchingFiles(DirectoryInfo dir, TagFilter filter)
            => GetMatchingFilesImpl(dir, filter);

        public TagFilter ParseFilterString(string filterString)
            => ParseFilterStringImpl(filterString);


        // Configurable implementations

        /// <summary>
        /// Default behavior is to always return an empty set of tags
        /// </summary>
        public Func<FileSystemInfo, IEnumerable<string>> GetTagsImpl =
            (file) => new string[] { };

        /// <summary>
        /// Default behavior is to throw a NotImplementedException
        /// </summary>
        public Func<FileSystemInfo, IEnumerable<string>, FileSystemInfo> SetTagsImpl =
            (file, tags) => throw new NotImplementedException();

        /// <summary>
        /// Default behavior is to throw a NotImplementedException
        /// </summary>
        public Func<DirectoryInfo, TagFilter, IEnumerable<FileSystemInfo>> GetMatchingFilesImpl =
            (dir, filter) => throw new NotImplementedException();

        /// <summary>
        /// Default behavior is to return a filter that
        /// always returns true.
        /// </summary>
        public Func<string, TagFilter> ParseFilterStringImpl =
            (filterString) => file => true;
    }
}
