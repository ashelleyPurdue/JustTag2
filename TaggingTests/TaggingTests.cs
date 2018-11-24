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
        public static void AssertFileTags(string fileName, params string[] expectedTags)
        {
            var file = new FileInfo(fileName);
            var actualTags = TagUtils.GetTags(file);

            var expectedSorted = expectedTags.OrderBy(f => f);
            var actualSorted = actualTags.OrderBy(f => f);

            Assert.IsTrue(actualSorted.SequenceEqual(expectedSorted));
        }

        [TestMethod] public void Untagged() => AssertFileTags("no_tags.txt");
        [TestMethod] public void JustFoo()  => AssertFileTags("file[foo].txt", "foo");
        [TestMethod] public void JustBar()  => AssertFileTags("file[bar].txt", "bar");
        [TestMethod] public void FooBar()   => AssertFileTags("file[foo bar].txt", "foo", "bar");
    }
}
