using System;
using System.IO;
using JustTag2;
using JustTag2.Tagging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LegacyTests
{
    [TestClass]
    public class FilterTests
    {
        private static ITaggingService TagUtils = new LegacyTaggingService();

        private static void AssertMatches(string fileName, string filter, bool shouldMatch = true)
        {
            var file = new FileInfo(fileName);
            var matchFunc = TagUtils.ParseFilterString(filter);

            Assert.AreEqual(shouldMatch, matchFunc(TagUtils.GetTags(file)));
        }

        [TestMethod] public void HasFoo()        => AssertMatches("file[foo].txt", "foo");
        [TestMethod] public void HasFooAndMore() => AssertMatches("file[foo bar].txt", "foo");
        [TestMethod] public void MissingFoo()    => AssertMatches("file[bar].txt", "foo", false);

        [TestMethod] public void LacksForbidden() => AssertMatches("file[foo].txt", "-forbidden");
        [TestMethod] public void HasForbidden()   => AssertMatches("file[forbidden].txt", "-forbidden", false);

        [TestMethod] public void HasFooAndForbidden() => AssertMatches("file[foo forbidden].txt", "foo -forbidden", false);
    }
}
