using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace JustTag2.Tests
{
    public class MainPageViewModelTests
    {
        [Theory]
        [InlineData(false, 20)]
        [InlineData(true, 20)]
        public void Folders_Are_Listed_Before_Files_After_Refresh(bool sortDescending, int numFiles)
        {
            // Generates a bunch of mixed files and folders
            IEnumerable<FileSystemInfo> GenerateTestFiles()
            {
                for (int i = 0; i < numFiles; i++)
                {
                    bool isDir = i % 2 == 0;
                    yield return isDir
                        ? new DirectoryInfo("" + i) as FileSystemInfo
                        : new FileInfo("" + i) as FileSystemInfo;
                }
            }

            // Configure a tag service that returns these dummy values
            var files = GenerateTestFiles().ToArray();
            var tagService = new MockTaggingService
            {
                GetMatchingFilesImpl = (file, filter) => files
            };

            // Make a view model and refresh it.
            var viewModel = new MainPageViewModel(tagService);
            viewModel.SortDescending = sortDescending;
            viewModel.Refresh();

            // Assert that all of the files are still there after the refresh.
            Assert.Equal(files.Length, viewModel.VisibleFiles.Length);

            // Assert that folders are always listed first
            bool shouldBeFolder = true;
            foreach (var file in viewModel.VisibleFiles)
            {
                // Once we encounter the first file, we expect
                // the rest of them to be files as well.
                if (file is FileInfo) 
                    shouldBeFolder = false;

                bool isFolder = file is DirectoryInfo;
                Assert.Equal(shouldBeFolder, isFolder);
            }
        }
    }
}
