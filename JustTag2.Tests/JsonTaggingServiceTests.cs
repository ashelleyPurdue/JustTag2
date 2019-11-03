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
    }
}
