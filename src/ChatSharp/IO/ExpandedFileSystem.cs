using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace ChatSharp.IO
{
    public class ExpandedFileSystem : IFileProvider
    {
        private readonly IFileProvider _baseProvider;
        private readonly string _subdirectory;

        public ExpandedFileSystem(string subdirectory, IFileProvider baseProvider)
        {
            _subdirectory = subdirectory;
            _baseProvider = baseProvider;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            // Adds the base path before getting directory contents
            return _baseProvider.GetDirectoryContents(Path.Combine(_subdirectory, subpath));
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            // Adds the base path before getting file info
            return _baseProvider.GetFileInfo(Path.Combine(_subdirectory, subpath));
        }

        public IChangeToken Watch(string filter)
        {
            // Adds the base path before watching
            return _baseProvider.Watch(Path.Combine(_subdirectory, filter));
        }
    }
}
