using ChatSharp.Engine;
using Microsoft.Extensions.FileProviders;
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

        public IFileProvider ContentRoot => _env.ContentRootFileProvider;
    }
}
