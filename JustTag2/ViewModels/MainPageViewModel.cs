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
        private readonly ITaggingService TagUtils;

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
                ? TagUtils.GetTags(SelectedFile)
                : null;

        public string FilterString
        {
            get => _filterString;
            set => this.SetAndRaise(ref _filterString, value, "FilterString");
        }
        private string _filterString;

        public MainPageViewModel(ITaggingService taggingService) : base()
        {
            TagUtils = taggingService;
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
            var filter = TagUtils.ParseFilterString(FilterString);
            var sortMethodKey = SortMethods.Keys.ToArray()[SelectedSortMethodIndex];
            var sortMethod = SortMethods[sortMethodKey];

            IEnumerable<FileSystemInfo> visibleFiles = CurrentFolder
                .EnumerateFiles()
                .Where(filter)
                .OrderBy(f => sortMethod(f));

            IEnumerable<FileSystemInfo> visibleFolders = CurrentFolder
                .EnumerateDirectories()
                .Where(filter)
                .OrderBy(f => sortMethod(f));   // TODO: Do something about this copypasta

            if (!SortDescending)
            {
                visibleFiles = visibleFiles.Reverse();
                visibleFolders = visibleFolders.Reverse();
            }

            VisibleFiles = visibleFolders
                .Concat(visibleFiles)
                .ToArray();
        }
    }
}
