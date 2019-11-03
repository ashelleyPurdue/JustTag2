using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using JustTag2.Tagging;

using Moq;
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

            var fs = new MockFileSystem();
            fs.MockFile
                .Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(jsonContent);
            fs.MockFile
                .Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            var tagService = new JsonTaggingService(fs);

            var expectedTags = new[] { "foo", "bar", "baz" };
            var actualTags = tagService.GetTags(new FileInfo("1000.txt"));

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
            string writtenText = tagFileStartingContents;

            var fs = new MockFileSystem();
            fs.MockFile
                .Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(() => writtenText != null);    // File exists if writtenText isn't null
            fs.MockFile
                .Setup(f => f.ReadAllText(It.IsAny<string>()))
                .Returns(() => writtenText);
            fs.MockFile
                .Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((path, text) =>
                {
                    writtenText = text;
                });

            var tagService = new JsonTaggingService(fs);
            tagService.SetTags(new FileInfo("foo.txt"), expectedTags);

            var actualTags = tagService.GetTags(new FileInfo("foo.txt"));
            Assert.True(expectedTags.SequenceEqual(actualTags));
        }
    }
}
