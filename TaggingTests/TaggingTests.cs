using System;
using System.Linq;
using System.IO;
using JustTag2.Tagging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaggingTests
{
    [TestClass]
    public class TaggingTests
    {
        public static void AssertTags(FileSystemInfo file, string[] expectedTags)
        {
            var actualTags = TagUtils.GetTags(file);

            var expectedSorted = expectedTags.OrderBy(f => f);
            var actualSorted = actualTags.OrderBy(f => f);

            Assert.IsTrue(actualSorted.SequenceEqual(expectedSorted));
        }

        public static void AssertFileTags(string fileName, params string[] expectedTags)
        {
            var file = new FileInfo(fileName);
            AssertTags(file, expectedTags);
        }

        public static void AssertFolderTags(string folderName, params string[] expectedTags)
        {
            string path = "../../TestFolderTemplate/tagged_folders/" + folderName;
            var dir = new DirectoryInfo(path);

            AssertTags(dir, expectedTags);
        }


        [TestMethod] public void Untagged() => AssertFileTags("no_tags.txt");
        [TestMethod] public void JustFoo()  => AssertFileTags("file[foo].txt", "foo");
        [TestMethod] public void JustBar()  => AssertFileTags("file[bar].txt", "bar");
        [TestMethod] public void FooBar()   => AssertFileTags("file[foo bar].txt", "foo", "bar");
    }
}
