using JustTag2.Tagging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace JustTag2
{
    public class MainPageViewModel : ViewModelBase
    {
        public delegate IComparable SortMethod(FileSystemInfo f);

        private static Random randGen = new Random();
        private readonly ITaggingService _taggingService;

        public int SelectedSortMethodIndex { get; set; } = 0;
        public Dictionary<string, SortMethod> SortMethods { get; set; } = new Dictionary<string, SortMethod>
        {
            {"Name", f => f.Name },
            {"Date", f => f.LastWriteTime },
            {"Shuffle", f => randGen.Next() }
        };

        public DirectoryInfo CurrentFolder
        {
            get => _currentFolder;
            set => this.SetAndRaise(ref _currentFolder, value, "CurrentFolder", "VisibleFiles", "SelectedIndex", "SelectedFile");
        }
        private DirectoryInfo _currentFolder = new DirectoryInfo(Directory.GetCurrentDirectory());
        public FileSystemInfo[] VisibleFiles
        {
            get => _visibleFiles;
            set => this.SetAndRaise(ref _visibleFiles, value, "VisibleFiles", "SelectedIndex", "SelectedFile");
        }
        private FileSystemInfo[] _visibleFiles;

        public bool SortDescending
        {
            get => _sortDescending;
            set => this.SetAndRaise(ref _sortDescending, value, "SortDescending");
        }
        private bool _sortDescending = true;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => this.SetAndRaise
            (
                ref _selectedIndex, 
                value, 
                "SelectedIndex", 
                "SelectedFile",
                "SelectedFileTags"
            );
        }
        private int _selectedIndex;

        public FileSystemInfo SelectedFile => (SelectedIndex < VisibleFiles.Length && SelectedIndex >= 0)
            ? VisibleFiles[SelectedIndex]
            : null;

        public IEnumerable<string>? SelectedFileTags => 
            (SelectedFile != null)
                ? _taggingService.GetTags(SelectedFile)
                : null;

        public string FilterString
        {
            get => _filterString;
            set => this.SetAndRaise(ref _filterString, value, "FilterString");
        }
        private string _filterString;

        public MainPageViewModel(ITaggingService taggingService) : base()
        {
            _taggingService = taggingService;
        }

        /// <summary>
        /// Selects the next file.  If previous is true, moves
        /// to the previous file instead
        /// </summary>
        /// <param name="previous"></param>
        public void SwipeToNextFile(bool previous)
        {
            int index = SelectedIndex;

            if (previous)
                index--;
            else
                index++;

            if (index < 0)
                index = VisibleFiles.Length - 1;
            if (index >= VisibleFiles.Length)
                index = 0;

            SelectedIndex = index;
        }

        public void Refresh()
        {
            var filter = _taggingService.ParseFilterString(FilterString);
            var sortMethodKey = SortMethods.Keys.ToArray()[SelectedSortMethodIndex];
            var sortMethod = SortMethods[sortMethodKey];

            var visibleItems = _taggingService
                .GetMatchingFiles(CurrentFolder, filter)
                .OrderBy(f => sortMethod(f));

            // Separate them into files and folders, so that way we can list
            // all the folder before the files
            var filesOnly = visibleItems
                .Where(f => f is FileInfo);

            var foldersOnly = visibleItems
                .Where(f => f is DirectoryInfo);

            if (!SortDescending)
            {
                filesOnly   = filesOnly.Reverse();
                foldersOnly = foldersOnly.Reverse();
            }

            // Recombine the lists, putting the folders first.
            VisibleFiles = foldersOnly
                .Concat(filesOnly)
                .ToArray();
        }
    }
}
