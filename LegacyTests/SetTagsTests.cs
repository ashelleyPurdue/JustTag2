using System;
using System.Linq;
using System.IO;
using JustTag2;
using JustTag2.Tagging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyTests
{
    [TestClass]
    public class SetTagsTests
    {
        private const string TEMPLATE_PATH = "../../../TestFolderTemplate";
        private const string TEST_PATH = "./TestFolder";

        private static ITaggingService TagUtils = new LegacyTaggingService();


        private static void AssertTagsChanged(string path, string expectedFileName, params string[] tags)
        {
            path = Path.Combine(TEST_PATH, path);

            FileSystemInfo file = FileSystemInfoExtensions.FromPath(path);
            file = TagUtils.SetTags(file, tags);

            // Assert that the name is changed
            Assert.AreEqual(expectedFileName, file.Name);

            // Assert that the tags are correct.
            var expectedTags = tags;
            var actualTags = TagUtils.GetTags(file);

            Assert.IsTrue(expectedTags.SequenceEqual(actualTags));
        }

        [TestInitialize]
        public void RestoreTemplateFolder()
        {
            // Delete the test folder if it exists
            if (Directory.Exists(TEST_PATH))
                Directory.Delete(TEST_PATH, true);

            // Restore it.
            FileSystemInfo dir = new DirectoryInfo(TEMPLATE_PATH);
            dir.Copy(TEST_PATH);
        }

        [TestMethod]
        public void FirstTagsOnFile() => AssertTagsChanged
        (
            "tagged_files/no_tags.txt",
            "no_tags[foo].txt",
            "foo"
        );

        [TestMethod]
        public void ReplaceBarWithFooFile() => AssertTagsChanged
        (
            "tagged_files/just_bar[bar].txt",
            "just_bar[foo].txt",
            "foo"
        );

        [TestMethod]
        public void FirstTagsOnFolder() => AssertTagsChanged
        (
            "tagged_folders/no_tags",
            "no_tags",
            "foo"
        );
    }
}
