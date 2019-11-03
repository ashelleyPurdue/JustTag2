using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Abstractions;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;

using Moq;

namespace JustTag2.Tests
{
    public class MockFileSystem : IFileSystem
    {
        public readonly Mock<IFile> MockFile = new Mock<IFile>();
        public IFile File => MockFile.Object;

        public readonly Mock<IDirectory> MockDirectory = new Mock<IDirectory>();
        public IDirectory Directory => MockDirectory.Object;

        // I see no reason why I would want to mock Path, since Path only
        // manipulates strings.  It doesn't write or read the actual file system.
        public IPath Path => new FileSystem().Path;


        // Not implemented stuff
        public IFileInfoFactory FileInfo => throw new NotImplementedException();
        public IFileStreamFactory FileStream => throw new NotImplementedException();
        public IDirectoryInfoFactory DirectoryInfo => throw new NotImplementedException();
        public IDriveInfoFactory DriveInfo => throw new NotImplementedException();
        public IFileSystemWatcherFactory FileSystemWatcher => throw new NotImplementedException();
    }

    
}
