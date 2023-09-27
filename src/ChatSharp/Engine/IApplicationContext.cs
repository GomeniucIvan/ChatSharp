using ChatSharp.IO;
using Microsoft.Extensions.FileProviders;

namespace ChatSharp.Engine
{
    public interface IApplicationContext
    {
        bool IsDatabaseInstalled { get; }
        IFileProvider ContentRoot { get; }
    }
}
