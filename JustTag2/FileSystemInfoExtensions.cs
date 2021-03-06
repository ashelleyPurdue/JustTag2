﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JustTag2
{
    // TODO: This file is duplicated accross JustTag2 and TabbedFileBrowser
    // Need to find a way to share it.
    /// <summary>
    /// Thanks, Microsoft. >:(
    /// </summary>
    public static class FileSystemInfoExtensions
    {
        public static void Move(this FileSystemInfo file, string dest)
        {
            switch (file)
            {
                case FileInfo f:        f.MoveTo(dest); break;
                case DirectoryInfo f:   f.MoveTo(dest); break;  // Am I seeing double?
            }
        }

        public static void Copy(this FileSystemInfo file, string dest)
        {
            switch (file)
            {
                case FileInfo f:        f.CopyTo(dest); break;
                case DirectoryInfo f:   DirectoryCopy(f.FullName, dest); break;
            }
        }

        public static string ParentFolderPath(this FileSystemInfo file)
        {
            switch (file)
            {
                case FileInfo f:        return f.DirectoryName;
                case DirectoryInfo f:   return f.Parent.FullName;

                default: throw new Exception("REALLY?!  Why would you add another FileSystemInfo?!");   
            }
        }

        /// <summary>
        /// Gets the parent directory of the given FileSystemInfo
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DirectoryInfo ParentDirectory(this FileSystemInfo file) => file switch
        {
            FileInfo f => f.Directory,
            _ => new DirectoryInfo(file.ParentFolderPath())
        };

        // Helper methods

        /// <summary>
        /// Creates a new FileSystemInfo from the specified path.
        /// If it's a file, it'll be a FileInfo.  If it's a directory,
        /// it'll be a DirectoryInfo.  No surprise there.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileSystemInfo FromPath(string path)
        {
            if (File.Exists(path))
                return new FileInfo(path);

            if (Directory.Exists(path))
                return new DirectoryInfo(path);

            throw new FileNotFoundException("The file or directory " + path + " could not be found.");
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Copied from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
            // Though I shouldn't *have* to copy it.  This shit should be built in!
            // Fuck you, Microsoft.

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // Copy the subdirectories
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath);
            }
        }
    }
}
