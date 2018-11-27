using System;
using System.Linq;
using System.IO;
using JustTag2.Tagging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaggingTests
{
    [TestClass]
    public class GetTagsTests
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


        [TestMethod] public void UntaggedFile() => AssertFileTags("no_tags.txt");
        [TestMethod] public void JustFooFile()  => AssertFileTags("file[foo].txt", "foo");
        [TestMethod] public void JustBarFile()  => AssertFileTags("file[bar].txt", "bar");
        [TestMethod] public void FooBarFile()   => AssertFileTags("file[foo bar].txt", "foo", "bar");

        [TestMethod] public void MissingJtffoldertags() => AssertFolderTags("missing_jtfoldertags");
        [TestMethod] public void UntaggedFolder() => AssertFolderTags("no_tags");
        [TestMethod] public void JustFooFolder()  => AssertFolderTags("just_foo", "foo");
        [TestMethod] public void JustBarFolder()  => AssertFolderTags("just_bar", "bar");
        [TestMethod] public void FooBarFolder()   => AssertFolderTags("foo_bar", "foo", "bar");
        [TestMethod] public void BarFooFolder()   => AssertFolderTags("bar_foo", "foo", "bar");

    }
}
