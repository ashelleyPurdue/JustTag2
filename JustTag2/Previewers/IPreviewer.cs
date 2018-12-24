using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;

namespace JustTag2.Previewers
{
    public interface IPreviewer
    {
        /// <summary>
        /// Returns the UserControl associated with this previewer.
        /// </summary>
        UserControl Control { get; }

        /// <summary>
        /// Returns true if the given file is supported by the previewer,
        /// false otherwise.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        bool CanPreview(FileSystemInfo file);

        /// <summary>
        /// Opens and displays the given file
        /// </summary>
        /// <param name="file"></param>
        Task Open(FileSystemInfo file);

        /// <summary>
        /// Closes the currently-open file
        /// </summary>
        Task Close();
    }
}
