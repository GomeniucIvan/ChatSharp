using ChatSharp.Engine;
using ChatSharp.IO;
using Microsoft.Extensions.Hosting;

namespace ChatSharp.Core.Data
{
    public class ApplicationContext : IApplicationContext
    {
        private readonly IHostEnvironment _env;
        private readonly ChatSharpDbContext _dbContext;

        public ApplicationContext(IHostEnvironment env, ChatSharpDbContext dbContext)
        {
            _env = env;
            _dbContext = dbContext;
        }

        public bool IsDatabaseInstalled => _dbContext.Database.CanConnect();

        public IFileSystem ContentRoot => (IFileSystem)_env.ContentRootFileProvider;
    }
}
