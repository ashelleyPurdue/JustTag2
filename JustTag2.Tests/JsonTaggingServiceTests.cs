using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using JustTag2.Tagging;

using Moq;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace JustTag2.Tests
{
    public class JsonTaggingServiceTests
    {
        [Fact]
        public void Reading_Tags_Works()
        {
            string jsonContent =
            @"
                {
                    ""1000.txt"": [""foo"", ""bar"", ""baz""]
                }
            ";

            var fs = new MockFileSystem
            (
                new Dictionary<string, MockFileData>()
                {
                    {"C:/.jtfiletags", new MockFileData(jsonContent) },
                    {"C:/1000.txt", "" }
                }
            );

            var tagService = new JsonTaggingService(fs);

            var expectedTags = new[] { "foo", "bar", "baz" };
            var actualTags = tagService.GetTags(new FileInfo("C:/1000.txt"));

            Assert.True(expectedTags.SequenceEqual(actualTags));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("{}")]
        [InlineData(@"{""foo.txt"": []}")]
        [InlineData(@"{""foo.txt"": [""fizz""]}")]
        public void Written_Tags_Can_Be_Read_Back(string tagFileStartingContents)
        {
            string[] expectedTags = new[]
            {
                "foo",
                "bar",
                "baz"
            };

            var existingFiles = new Dictionary<string, MockFileData>()
            {
                {"C:/foo.txt", new MockFileData("") }
            };

            if (tagFileStartingContents != null)
                existingFiles.Add("C:/.jtfiletags", new MockFileData(tagFileStartingContents));

            var fs = new MockFileSystem(existingFiles);
            var tagService = new JsonTaggingService(fs);
            tagService.SetTags(new FileInfo("C:/foo.txt"), expectedTags);

            var actualTags = tagService.GetTags(new FileInfo("C:/foo.txt"));
            Assert.True(expectedTags.SequenceEqual(actualTags));
        }
    
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("{}")]
        [InlineData(@"{""foo.txt"": []}")]
        [InlineData(@"{""bar.txt"": [""fizz"", ""buzz""]}")]
        public void Foo_Dot_Txt_Should_Have_No_Tags(string tagFileContents)
        {
            var existingFiles = new Dictionary<string, MockFileData>()
            {
                { "C:/foo.txt", new MockFileData("") }
            };
            if (tagFileContents != null)
                existingFiles.Add("C:/.jtfiletags", tagFileContents);

            var fs = new MockFileSystem(existingFiles);
            var tagService = new JsonTaggingService(fs);
            var tags = tagService.GetTags(new FileInfo("C:/foo.txt"));

            Assert.Empty(tags);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("{}")]
        [InlineData(@"{""foo.txt"": []}")]
        [InlineData(@"{""bar.txt"": [""fizz"", ""buzz""]}")]
        public void Foo_Dot_Txt_Should_Be_Considered_Untagged(string tagFileContents)
        {
            var existingFiles = new Dictionary<string, MockFileData>()
            {
                {"C:/foo.txt", new MockFileData("") }
            };
            if (tagFileContents != null)
                existingFiles.Add("C:/.jtfiletags", tagFileContents);

            var fs = new MockFileSystem(existingFiles);

            ITaggingService tagService = new JsonTaggingService(fs);
            TagFilter untaggedFilter = tagService.ParseFilterString(":untagged:");

            var matchingFiles = tagService.GetMatchingFiles
            (
                new DirectoryInfo("C:/"),
                untaggedFilter
            );

            Assert.Contains(matchingFiles, f => f.Name == "foo.txt");
        }
    }
}
