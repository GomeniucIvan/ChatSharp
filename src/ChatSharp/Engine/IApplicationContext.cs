using ChatSharp.IO;

namespace ChatSharp.Engine
{
    public interface IApplicationContext
    {
        bool IsDatabaseInstalled { get; }
        IFileSystem ContentRoot { get; }
    }
}
