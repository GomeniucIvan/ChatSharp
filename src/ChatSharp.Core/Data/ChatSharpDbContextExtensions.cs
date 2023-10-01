using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace ChatSharp.Core.Data
{
    public class ChatSharpDbContextExtensions
    {
        public static DbConnectionStringBuilder CreateConnectionStringBuilder(string connectionString)
            => new SqlConnectionStringBuilder(connectionString);

        public static DbConnectionStringBuilder CreateConnectionStringBuilder(
            string server,
            string database,
            string userId,
            string password)
        {
            var builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = string.IsNullOrEmpty(userId),
                DataSource = server,
                InitialCatalog = database,
                UserInstance = false,
                Pooling = true,
                MinPoolSize = 1,
                MaxPoolSize = 1024,
                Enlist = false,
                MultipleActiveResultSets = true,
                Encrypt = false
            };

            if (!builder.IntegratedSecurity)
            {
                builder.UserID = userId;
                builder.Password = password;
            }

            return builder;
        }
    }
}
